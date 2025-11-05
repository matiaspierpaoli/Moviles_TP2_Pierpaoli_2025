using UnityEngine;
using UnityEngine.InputSystem;
using Game.Core.Systems;
using Game.Core.Data;
using Game.Core.GameplayModel;
using Game.Core.ServicesCore;

using Game.External;

class CalibratedInput : IInputStrategy
{
    readonly IInputStrategy src; Vector2 bias;
    public CalibratedInput(IInputStrategy s){ src = s; }
    public void Calibrate(){ bias = src.ReadTilt(); }
    public Vector2 ReadTilt(){ return src.ReadTilt() - bias; }
}

class SmoothedInput : IInputStrategy
{
    readonly IInputStrategy src; readonly float k; Vector2 f;
    public SmoothedInput(IInputStrategy s, float smoothing01 = 0.15f){ src = s; k = Mathf.Clamp01(smoothing01); }
    public Vector2 ReadTilt(){ f = Vector2.Lerp(f, src.ReadTilt(), 1f - Mathf.Exp(-k * 60f * Time.deltaTime)); return f; }
}

namespace Game.Controller.Gameplay
{
    public class QuickGameplayRunner : MonoBehaviour
    {
        [Header("Refs")]
        public BoardView view;
        public LevelData level;
        public DifficultyCurve curve;

        [Header("Opciones")]
        public bool landscapeRight = true;
        public bool autoCalibrate = true;
        public float smoothing = 0.15f;

        LevelController ctrl;
        SessionTimer timer;
        DifficultyService diff;
        IInputStrategy input;
        CalibratedInput cin;

        void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
    var acc = new AccelerometerInput(ScreenOrientation.LandscapeLeft);
    acc.CalibrateNeutral();
    var baseInput = acc;
    IHaptics h = new AndroidHaptics();
#else
            var baseInput = new EditorInput();
            IHaptics h = new NoopHaptics();
#endif
            cin = new CalibratedInput(baseInput);
            if (autoCalibrate) cin.Calibrate();
            input = new SmoothedInput(cin, smoothing);

            diff = new DifficultyService(curve);
            var m = new LevelModel(level);

            ctrl = new LevelController(m, view, input, h, diff);
            ctrl.StartLevel();

            timer = new SessionTimer(level.sessionSeconds);
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
                cin.Calibrate();
#endif
        }

        void FixedUpdate()
        {
            timer.Tick();
            ctrl.Tick(timer.Normalized);
        }
    }
}
