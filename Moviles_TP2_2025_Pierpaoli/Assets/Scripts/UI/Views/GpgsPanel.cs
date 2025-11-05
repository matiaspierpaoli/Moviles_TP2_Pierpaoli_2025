using UnityEngine;
using Game.Core.ServicesCore;

namespace Game.UI
{
    public class GpgsPanel : MonoBehaviour
    {
        public void TapLogin()
        {
            Services.Google?.Authenticate(s => Debug.Log("Login: " + s));
        }

        public void TapAch()
        {
            Services.Google?.Increment(GPGSIds.achievement_first_open, 5);
        }

        public void TapShow()
        {
            Services.Google?.ShowUI();
        }
    }
}