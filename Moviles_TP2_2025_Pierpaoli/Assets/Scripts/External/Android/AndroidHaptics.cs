using UnityEngine;
using Game.Core.ServicesCore;

namespace Game.External
{
    public class AndroidHaptics : IHaptics
    {
        public void Hit()
        {
            Handheld.Vibrate();
        }

        public void Goal()
        {
            Handheld.Vibrate();
        }
    }
}