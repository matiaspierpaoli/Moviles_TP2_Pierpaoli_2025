using UnityEngine;
using Game.Core.ServicesCore;

namespace Game.External
{
    public class AndroidHaptics : IHaptics
    {
        public void PlaySimpleVibration()
        {
            Handheld.Vibrate();
        }
    }
}