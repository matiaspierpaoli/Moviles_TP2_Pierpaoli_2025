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
        
        [Header("Opciones de Input - Android")]
        public float rollSensitivityAndroid = 45f;
        public float pitchSensitivityAndroid = 45f;
        public float smoothing = 0.1f;
        public float deadzone = 0.05f; 
        
        [Header("Opciones de Input - Editor")]
        public float boardMoveSpeed = 2f;
        public float boardReturnSpeed = 3f;
        
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
            SaveSystem.Load(model);

#if UNITY_ANDROID && !UNITY_EDITOR
            smartInput = new SmartInput(smoothing, deadzone, rollSensitivityAndroid, pitchSensitivityAndroid);
            inputStrategy = smartInput;
            haptics       = new AndroidHaptics();
            logs          = new AndroidLogService();
            google        = new AndroidGpgsService();
            google.Init();
            Services.Google = google;
            
#else
            inputStrategy = new EditorInput(boardMoveSpeed, boardReturnSpeed);
            haptics       = new NoopHaptics();
            logs          = new NoopLogService();
            google        = new DummyGpgsService();
#endif

            
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
        
        public void Ui_PlayLevel(int level)
        {
            model.SetLevel(level);
            SceneTransit.SetNext(typeof(GameplayState), levelProfile);
            Go<LoadingState>();
        }
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
