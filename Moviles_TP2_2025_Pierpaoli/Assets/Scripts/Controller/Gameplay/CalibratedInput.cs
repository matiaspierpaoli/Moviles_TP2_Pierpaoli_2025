using UnityEngine;
using Game.Core.ServicesCore;

namespace Game.Controller.Gameplay
{
    public class CalibratedInput : IInputStrategy
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
        {
            bias = src.ReadRawTilt() - neutral;
        }

        public void CalibrateToCurrent()
        {
            bias = src.ReadRawTilt();
        }
    
        public void SetNeutral(Vector2 newNeutral)
        {
            neutral = newNeutral;
            bias = src.ReadRawTilt() - neutral;
        }
        public Vector2 ReadTilt(){ return src.ReadTilt() - bias; }
        public Vector2 ReadRawTilt(){ return src.ReadRawTilt() - bias; }
    }
}