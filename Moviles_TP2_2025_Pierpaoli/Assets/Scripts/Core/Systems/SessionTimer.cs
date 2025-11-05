using UnityEngine;

namespace Game.Core.Systems
{
    public class SessionTimer
    {
        readonly float total;
        float t;

        public SessionTimer(int seconds)
        {
            total = Mathf.Max(30, seconds);
            t = total;
        }

        public void Reset()
        {
            t = total;
        }

        public void Tick()
        {
            t -= Time.deltaTime;
        }

        public float Normalized => Mathf.InverseLerp(total, 0f, t);
        public bool IsOver => t <= 0f;
    }
}