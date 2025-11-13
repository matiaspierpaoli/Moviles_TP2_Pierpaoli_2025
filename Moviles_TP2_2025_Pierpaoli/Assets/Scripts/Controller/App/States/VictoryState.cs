using UnityEngine;
using Game.UI.Views;
using Game.Core.Data;

namespace Game.Controller
{
    public class VictoryState : BaseState
    {
        private readonly AppModel model;

        public VictoryState(AppController a, ScreenView v, AppModel m) : base(a, v)
        {
            model = m;
        }

        public override void Enter()
        {
            base.Enter();
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Screen.orientation !=  ScreenOrientation.Portrait)
                Screen.orientation = ScreenOrientation.Portrait;
#endif

            VictoryView victoryUI = view as VictoryView;
            if (victoryUI == null) return;

            int session = model.lastSessionCoins;
            int total = model.coins;

            victoryUI.ShowResults(session, total);
        }
    }
}