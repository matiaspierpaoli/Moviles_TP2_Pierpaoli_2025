using UnityEngine;
using Game.UI.Views;

namespace Game.Controller
{
    public class MainMenuState : BaseState
    {
        public MainMenuState(AppController a, ScreenView v) : base(a, v)
        {
        }

        public override void Enter()
        {
            base.Enter();
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Screen.orientation !=  ScreenOrientation.Portrait)
                Screen.orientation = ScreenOrientation.Portrait;
#endif
        }
    }
}