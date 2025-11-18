using Game.Core.Data;
using Game.UI.Views;
using Game.Core.Systems;
using Game.External;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controller
{
    public class StoreState : BaseState
    {
        private readonly AppModel model;
        private Economy econ;
        private EconomyConfig econCfg;
        private BallMaterialConfig ballMaterialCfg;
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
            ballMaterialCfg = AssetLoader.LoadBallMaterialConfig();
            econ = new Economy(model, econCfg, ballMaterialCfg);

            storeView.buyHiddenLevelButton.onClick.RemoveAllListeners();
            storeView.buyHiddenLevelButton.onClick.AddListener(OnBuyHiddenLevelClicked);

            if (app.backgroundImage != null)
            {
                app.backgroundImage.enabled = false;
            }
            
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

        private void OnMaterialButtonClicked(string materialId)
        {
            if (model.ownedBallMaterialIds.Contains(materialId))
            {
                econ.SelectMaterial(materialId);
            }
            else
            {
                bool success = econ.TryBuyMaterial(materialId);
                if (success) { app.haptics.PlaySimpleVibration(); }
            }
            RefreshUI();
        }
        
        private void OnBuyHiddenLevelClicked()
        {
            bool success = econ.TryBuyHiddenLevel();

            if (success)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                if (app.google != null)
                {
                    app.google.Unlock(GPGSIds.achievement_last_dance);
                    Debug.Log("Achievement unlocked: Last dance ");
                }
#endif
                app.haptics.PlaySimpleVibration();
            }

            RefreshUI();
        }

        private void RefreshUI()
        {
            storeView.UpdateGeneralUI(model.coins, econCfg.hiddenLevelPrice, model.hiddenLevelUnlocked);
        
            storeView.SetupMaterialButtons(
                ballMaterialCfg.materialItems,
                model.coins,
                model.selectedBallMaterialId,
                model.ownedBallMaterialIds,
                OnMaterialButtonClicked
            );
        }
    }
}