using UnityEngine;
using Game.Core.Data;
using Game.Core.ServicesCore;
using Game.UI.Views;
using Game.UI.Strategy;
using Game.External;
using Game.Controller.Gameplay;

namespace Game.Controller
{
    public class AppController : MonoBehaviour
    {
        [Header("Model")] public AppModel model;
        [Header("Views")] public ScreenView bootView, mainMenuView, levelSelectView, gameplayView, victoryView;

        AppStateMachine fsm;
        ITransitionStrategy transition;
        
        [Header("Opciones de Input")]
        public float smoothing = 0.1f;
        public Vector2 neutralTarget = new Vector2(0f, 0.35f); // Inclinación default
        public bool autoCalibrateOnStart = false;
        
        public CalibratedInput cin { get; private set; }
        public SmoothedInput smoothedInput { get; private set; }

        public IInputStrategy inputStrategy { get; private set; }
        public IHaptics       haptics       { get; private set; }
        public ILogService    logs          { get; private set; }
        public IGoogleService google        { get; private set; }

        void Awake()
        {
            fsm = new AppStateMachine();
            transition = new FadeTransition(0.2f);

#if UNITY_ANDROID && !UNITY_EDITOR
            inputStrategy = new AccelerometerInput();
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

            cin = new CalibratedInput(inputStrategy, neutralTarget);
            smoothedInput = new SmoothedInput(cin, smoothing);
            
            // 3. Asignamos el input FINAL (suavizado) a la propiedad pública
            inputStrategy = smoothedInput;
            
            fsm.Register(new BootState(this, bootView));
            fsm.Register(new MainMenuState(this, mainMenuView));
            fsm.Register(new LevelSelectState(this, levelSelectView, model));
            fsm.Register(new GameplayState(this, gameplayView, model));
            fsm.Register(new VictoryState(this, victoryView, model));
        }

        void Start()
        {
            HideAll(); 
            fsm.Change<BootState>();
        }
        public void Go<T>() where T : IAppState => fsm.Change<T>();
        public void Swap(ScreenView from, ScreenView to) => StartCoroutine(transition.Play(from, to));

        void FixedUpdate()
        {
            fsm?.Tick();
        }
        
        void HideAll()
        {
            bootView?.Hide(); 
            mainMenuView?.Hide(); 
            levelSelectView?.Hide(); 
            gameplayView?.Hide();
            victoryView?.Hide();
        }

        // UI hooks
        public void Ui_ToMenu()   => Go<MainMenuState>();
        public void Ui_ToSelect() => Go<LevelSelectState>();
        public void Ui_Play1(){ model.SetLevel(1); Go<GameplayState>(); }
        public void Ui_Play2(){ model.SetLevel(2); Go<GameplayState>(); }
        public void Ui_Play3(){ model.SetLevel(3); Go<GameplayState>(); }
        public void Ui_Play4(){ model.SetLevel(4); Go<GameplayState>(); }
        public void Ui_ResetCalibration()
        {
            cin?.CalibrateToCurrent();
            smoothedInput?.ResetToCurrent();
        }
        public void Ui_ExitGame(){ Application.Quit(); }
    }
}
