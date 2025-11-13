using Game.Core.Data;
using System.Linq;

namespace Game.Core.Systems
{
    public class Economy
    {
        readonly AppModel model; 
        readonly EconomyConfig cfg;
        readonly BallMaterialConfig materialCfg;

        public Economy(AppModel m, EconomyConfig c, BallMaterialConfig mc)
        {
            model = m; 
            cfg = c;
            materialCfg = mc;
        }

        public bool TryBuy(int price)
        {
            if (model.coins < price) 
                return false; 
            model.coins -= price; 
            SaveSystem.Save(model); 
            return true;
        }
        
        public bool TryBuyHiddenLevel()
        {
            if (model.hiddenLevelUnlocked) return false;
            if (model.coins < cfg.hiddenLevelPrice) return false;

            model.coins -= cfg.hiddenLevelPrice;
            model.hiddenLevelUnlocked = true;
            SaveSystem.Save(model);
            return true;
        }
        
        public BallMaterialConfig.MaterialItem GetSelectedMaterialItem()
        {
            return materialCfg.materialItems.FirstOrDefault(item => item.id == model.selectedBallMaterialId);
        }

        public bool TryBuyMaterial(string materialId)
        {
            BallMaterialConfig.MaterialItem itemToBuy = materialCfg.materialItems.FirstOrDefault(item => item.id == materialId);
            if (itemToBuy == null) return false;

            if (model.ownedBallMaterialIds.Contains(materialId)) return false; 
            if (model.coins < itemToBuy.price) return false; 

            model.coins -= itemToBuy.price;
            model.AddOwnedMaterial(materialId);
            SaveSystem.Save(model);
            return true;
        }

        public bool SelectMaterial(string materialId)
        {
            if (!model.ownedBallMaterialIds.Contains(materialId)) return false; 

            model.selectedBallMaterialId = materialId;
            SaveSystem.Save(model); 
            return true;
        }
        
        public void AddSessionCoins(int collectedCount)
        {
            if (collectedCount <= 0) return;

            int amount = collectedCount * cfg.rewardPerCoin; 
            model.coins += amount;
            SaveSystem.Save(model);
        }
        
        public bool HasMaterial(string materialId)
        {
            return model.ownedBallMaterialIds.Contains(materialId);
        }
    }
}