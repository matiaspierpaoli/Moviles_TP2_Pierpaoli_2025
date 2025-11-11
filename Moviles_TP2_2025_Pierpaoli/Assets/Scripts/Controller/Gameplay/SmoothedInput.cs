using UnityEngine;
using Game.Core.ServicesCore;

namespace Game.Controller.Gameplay
{
    public class SmoothedInput : IInputStrategy
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
}