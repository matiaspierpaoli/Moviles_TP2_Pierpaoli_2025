using System;
using UnityEngine;
using Game.Core.Data;
using Game.Core.ServicesCore;
using Game.UI.Views;
using Game.UI.Strategy;
using Game.External;
using Game.Core.Systems;
using UnityEngine.UI;

namespace Game.Controller
{
    public class AppController : MonoBehaviour
    {
        [Header("Model")] public AppModel model;
        [Header("Views")] public ScreenView mainMenuView, levelSelectView, gameplayView, victoryView, storeView, loadingView, logView;

        [Header("Loading Profiles")]
        public LoadingProfile levelProfile;
        
        public Image backgroundImage;
        
        [NonSerialized]
        public AppStateMachine fsm;
        
        ITransitionStrategy transition;
        
        [Header("Opciones de Input")]
        public float rollSensitivity = 45f;
        public float pitchSensitivity = 45f;
        public float smoothing = 0.1f;
        public float deadzone = 0.05f;
        
        public SmartInput smartInput       { get; private set; }
        public IInputStrategy inputStrategy { get; private set; }
        public IHaptics       haptics       { get; private set; }
        public ILogService    logs          { get; private set; }
        public IGoogleService google        { get; private set; }

        void Awake()
        {
            fsm = new AppStateMachine();
            transition = new FadeTransition(0.2f);
            
            backgroundImage.enabled = false;

#if UNITY_ANDROID && !UNITY_EDITOR
            smartInput = new SmartInput(smoothing, deadzone, rollSensitivity, pitchSensitivity);
            inputStrategy = smartInput;
            haptics       = new AndroidHaptics();
            logs          = new AndroidLogService();
            google        = new AndroidGpgsService();
#else
            inputStrategy = new EditorInput();
            haptics       = new NoopHaptics();
            logs          = new NoopLogService();
            google        = new DummyGpgsService();
#endif
            // google.Init();
            // google.Authenticate(s => Debug.Log("GPGS: " + s));
            // Services.Google = google;

            SaveSystem.Load(model);
            
            fsm.Register(new MainMenuState(this, mainMenuView));
            fsm.Register(new LevelSelectState(this, levelSelectView, model));
            fsm.Register(new GameplayState(this, gameplayView, model));
            fsm.Register(new VictoryState(this, victoryView, model));
            fsm.Register(new StoreState(this, storeView, model));
            fsm.Register(new LoadingState(this, loadingView));
            fsm.Register(new LogViewState(this, logView));
        }

        void Start()
        {
            HideAll(); 
            fsm.Change<MainMenuState>();
        }
        public void Go<T>() where T : IAppState => fsm.Change<T>();
        public void Swap(ScreenView from, ScreenView to) => StartCoroutine(transition.Play(from, to));

        void FixedUpdate()
        {
            fsm?.Tick();
        }
        
        void HideAll()
        {
            mainMenuView?.Hide(); 
            levelSelectView?.Hide(); 
            gameplayView?.Hide();
            victoryView?.Hide();
            storeView?.Hide();
            loadingView?.Hide();
            logView?.Hide();
        }

        // UI hooks
        public void Ui_ToMenu()   => Go<MainMenuState>();
        public void Ui_ToStore()  => Go<StoreState>();
        public void Ui_ToSelect() => Go<LevelSelectState>();
        public void Ui_ToLogs() => Go<LogViewState>();
        public void Ui_Play1(){ model.SetLevel(2); SceneTransit.SetNext(typeof(GameplayState), levelProfile); Go<LoadingState>(); }
        public void Ui_Play2(){ model.SetLevel(3); SceneTransit.SetNext(typeof(GameplayState), levelProfile); Go<LoadingState>(); }
        public void Ui_Play3(){ model.SetLevel(4); SceneTransit.SetNext(typeof(GameplayState), levelProfile); Go<LoadingState>(); }
        public void Ui_Play4(){ model.SetLevel(5); SceneTransit.SetNext(typeof(GameplayState), levelProfile); Go<LoadingState>(); }
        public void Ui_ResetCalibration()
        {
            smartInput?.CalibrateToCurrent();
        }
        
        public void Ui_StartTutorial()
        {
            model.startTutorial = true;
            model.SetLevel(1);
        
            SceneTransit.SetNext(typeof(GameplayState), levelProfile);
            Go<LoadingState>();
        }
        
        public void Ui_RestartLevel()
        {
            fsm.Change<GameplayState>(force: true);
        }
        
        public void Ui_ResetProgress()
        {
            SaveSystem.ClearSave();
            model.ResetToDefaults();
            Go<MainMenuState>();
        }
        public void Ui_ExitGame(){ Application.Quit(); }
    }
}
