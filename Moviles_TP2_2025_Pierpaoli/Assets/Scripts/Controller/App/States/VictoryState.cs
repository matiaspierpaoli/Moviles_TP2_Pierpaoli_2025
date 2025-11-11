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

            VictoryView victoryUI = view as VictoryView;
            if (victoryUI == null) return;

            int session = model.lastSessionCoins;
            int total = model.coins;

            victoryUI.ShowResults(session, total);
        }
    }
}