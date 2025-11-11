using UnityEngine;
using UnityEngine.InputSystem; 
using Game.Core.ServicesCore;

namespace Game.External
{
    public class SmartInput : IInputStrategy
    {
        private readonly AttitudeSensor sensor;

        private Quaternion calibrationBias;

        private readonly float k; 
        private Vector2 f; 

        private readonly float deadzone;
        
        private readonly float rollSensitivity; 
        private readonly float pitchSensitivity; 

        public SmartInput(float smoothing = 0.1f, float deadzone = 0.05f, float rollSensitivity = 45f, float pitchSensitivity = 45f)
        {
            sensor = AttitudeSensor.current;
            if (sensor != null && !sensor.enabled)
            {
                InputSystem.EnableDevice(sensor);
            }

            k = Mathf.Clamp01(smoothing);
            this.deadzone = deadzone;

            this.rollSensitivity = rollSensitivity;
            this.pitchSensitivity = pitchSensitivity;
            
            CalibrateToCurrent();
        }

        public void CalibrateToCurrent()
        {
            if (sensor == null) return;
            
            calibrationBias = sensor.attitude.ReadValue();
            
            ResetToCurrent();
        }

        public void ResetToCurrent()
        {
            f = ReadRawTilt(); 
        }

        public Vector2 ReadRawTilt()
        {
            if (sensor == null) return Vector2.zero;

            Quaternion currentAttitude = sensor.attitude.ReadValue();
            Quaternion relativeRotation = Quaternion.Inverse(calibrationBias) * currentAttitude;
            Vector3 euler = relativeRotation.eulerAngles;

            float pitch = (euler.x > 180) ? euler.x - 360 : euler.x; 
            float roll = (euler.y > 180) ? euler.y - 360 : euler.y;

            float tiltY = Mathf.Clamp(pitch / pitchSensitivity, -1f, 1f);
            float tiltX = Mathf.Clamp(roll / rollSensitivity, -1f, 1f);

            return new Vector2(-tiltX, -tiltY);
        }
        
        public Vector2 ReadTilt()
        {
            Vector2 raw = ReadRawTilt();
            
            float finalX = (Mathf.Abs(raw.x) < deadzone) ? 0f : raw.x;
            float finalY = (Mathf.Abs(raw.y) < deadzone) ? 0f : raw.y;
            Vector2 deadzoned = new Vector2(finalX, finalY);

            f = Vector2.Lerp(f, deadzoned, 1f - Mathf.Exp(-k * 60f * Time.deltaTime));
            return f;
        }
    }
}