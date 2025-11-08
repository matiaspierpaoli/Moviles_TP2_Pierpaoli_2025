using UnityEngine;
using UnityEngine.InputSystem;
using Game.Core.Systems;
using Game.Core.Data;
using Game.Core.GameplayModel;
using Game.Core.ServicesCore;

using Game.External;

class CalibratedInput : IInputStrategy
{
    readonly IInputStrategy src;
    Vector2 neutral;
    Vector2 bias;

    public CalibratedInput(IInputStrategy s, Vector2 neutralTarget)
    {
        src = s;
        neutral = neutralTarget;
    }

    public void CalibrateToTarget()
    { bias = src.ReadRawTilt() - neutral; }

    public void CalibrateToCurrent()
    { bias = src.ReadRawTilt(); }
    
    public void SetNeutral(Vector2 newNeutral)
    {
        neutral = newNeutral;
        bias = src.ReadRawTilt() - neutral;
    }
    public Vector2 ReadTilt(){ return src.ReadTilt() - bias; }
    public Vector2 ReadRawTilt(){ return src.ReadRawTilt() - bias; }
}

class SmoothedInput : IInputStrategy
{
    readonly IInputStrategy src; 
    readonly float k; 
    Vector2 f;

    public SmoothedInput(IInputStrategy s, float smoothing01 = 0.15f)
    {
        src = s; 
        k = Mathf.Clamp01(smoothing01);
        f =  src.ReadRawTilt();
    }
    public void ResetToCurrent() { f = src.ReadTilt(); }
    
    public Vector2 ReadTilt()
    {
        f = Vector2.Lerp(f, src.ReadTilt(), 1f - Mathf.Exp(-k * 60f * Time.deltaTime));
        return f;
    }
    
    public Vector2 ReadRawTilt()
    {
        return src.ReadRawTilt();
    }
}

namespace Game.Controller.Gameplay
{
    public class QuickGameplayRunner : MonoBehaviour
    {
        [Header("Refs")]
        public BoardView view;
        public LevelData level;

        [Header("Opciones")]
        public bool autoCalibrate = true;
        public float smoothing = 0.1f;

        LevelController ctrl;
        SessionTimer timer;
        DifficultyService diff;
        IInputStrategy input;
        CalibratedInput cin;
        SmoothedInput smoothedInput;

        void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var baseInput = new AccelerometerInput(ScreenOrientation.LandscapeLeft);
            IHaptics h = new AndroidHaptics();
#else
            var baseInput = new EditorInput();
            IHaptics h = new NoopHaptics();
#endif
            cin = new CalibratedInput(baseInput, new Vector2(0f, 0.35f));
            if (autoCalibrate) 
                cin.CalibrateToTarget();
            else
                cin.CalibrateToCurrent();
            
            smoothedInput = new SmoothedInput(cin, smoothing);
            input = smoothedInput;

            diff = new DifficultyService(level.parameters);
            var m = new LevelModel(level);

            ctrl = new LevelController(m, view, input, h, diff);
            ctrl.StartLevel();

            timer = new SessionTimer(level.sessionSeconds);
        }

        public void ResetCalibration()
        {
            cin.CalibrateToCurrent();
            smoothedInput?.ResetToCurrent();
        }
        
        void FixedUpdate()
        {
            timer.Tick();
            ctrl.Tick(timer.Normalized);
        }
    }
}
