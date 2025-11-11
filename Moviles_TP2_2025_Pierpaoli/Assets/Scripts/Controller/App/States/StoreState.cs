using Game.Core.Data;
using Game.UI.Views;
using Game.Core.Systems;

namespace Game.Controller
{
    public class StoreState : BaseState
    {
        private readonly AppModel model;
        private Economy econ;
        private EconomyConfig econCfg;
        private StoreView storeView;

        public StoreState(AppController a, ScreenView v, AppModel m) : base(a, v)
        {
            model = m;
        }

        public override void Enter()
        {
            base.Enter();

            storeView = view as StoreView;
            if (storeView == null) return;

            econCfg = AssetLoader.LoadEconomy();
            econ = new Economy(model, econCfg);

            storeView.buyHiddenLevelButton.onClick.AddListener(OnBuyHiddenLevelClicked);

            RefreshUI();
        }

        public override void Exit()
        {
            if (storeView != null)
            {
                storeView.buyHiddenLevelButton.onClick.RemoveListener(OnBuyHiddenLevelClicked);
            }
            base.Exit();
        }

        private void OnBuyHiddenLevelClicked()
        {
            bool success = econ.TryBuyHiddenLevel();

            if (success)
            {
                app.haptics.Goal();
            }

            RefreshUI();
        }

        private void RefreshUI()
        {
            storeView.UpdateView(model.coins, econCfg.hiddenLevelPrice, model.hiddenLevelUnlocked);
        }
    }
}