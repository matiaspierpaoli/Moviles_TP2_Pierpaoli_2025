using UnityEngine;
using UnityEngine.InputSystem; 
using Game.Core.ServicesCore;

namespace Game.External
{
    public class AccelerometerInput : IInputStrategy
    {
        readonly ScreenOrientation orient;
        readonly float maxPlayerTiltAngle = 15f;
        readonly float maxG;
        readonly float deadzone = 0.05f;

        public AccelerometerInput(ScreenOrientation orient = ScreenOrientation.LandscapeLeft)
        {
            this.orient = orient;
            if (Accelerometer.current != null && !Accelerometer.current.enabled)
                InputSystem.EnableDevice(Accelerometer.current);
            
            this.maxG = Mathf.Sin(maxPlayerTiltAngle * Mathf.Deg2Rad);
        }

        public Vector2 ReadTilt()
        {
            Vector2 tilt = ReadRawTilt();
            
            float finalX = (Mathf.Abs(tilt.x) < deadzone) ? 0f : tilt.x;
            float finalY = (Mathf.Abs(tilt.y) < deadzone) ? 0f : tilt.y;

            return new Vector2(finalX, finalY);
        }

        public Vector2 ReadRawTilt()
        {
            var a = ReadRaw();
            float rawPitch = a.y;
            float rawRoll = a.x;
            
            float rawTiltX_Roll, rawTiltY_Pitch;
            
            if (this.orient == ScreenOrientation.LandscapeLeft)
            {
                rawTiltX_Roll = rawRoll; 
                rawTiltY_Pitch = rawPitch;
            }
            else
            {
                rawTiltX_Roll = -rawRoll;
                rawTiltY_Pitch = -rawPitch;
            }

            float tiltX = Mathf.Clamp(rawTiltX_Roll / maxG, -1f, 1f);
            float tiltY = Mathf.Clamp(rawTiltY_Pitch / maxG, -1f, 1f);

            return new Vector2(tiltX, tiltY);
        }
        
        static Vector3 ReadRaw()
        {
            return Accelerometer.current != null
                ? Accelerometer.current.acceleration.ReadValue()
                : Vector3.zero;
        }
    }
}