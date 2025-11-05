using UnityEngine;
using UnityEngine.InputSystem; 
using Game.Core.ServicesCore;

namespace Game.External
{
    public class AccelerometerInput : IInputStrategy
    {
        readonly ScreenOrientation orient;
        Vector3 bias;
        bool hasBias;

        public AccelerometerInput(ScreenOrientation orient = ScreenOrientation.LandscapeRight)
        {
            this.orient = orient;
            if (Accelerometer.current != null && !Accelerometer.current.enabled)
                InputSystem.EnableDevice(Accelerometer.current);
        }

        public void CalibrateNeutral()
        {
            var a = ReadRaw();
            bias = a;
            hasBias = true;
        }

        public Vector2 ReadTilt()
        {
            var a = Accelerometer.current?.acceleration.ReadValue() ?? Vector3.zero;
    		return new Vector2(a.y, a.x);
        }

        static Vector3 ReadRaw()
        {
            return Accelerometer.current != null
                ? Accelerometer.current.acceleration.ReadValue()
                : Vector3.zero;
        }
    }
}