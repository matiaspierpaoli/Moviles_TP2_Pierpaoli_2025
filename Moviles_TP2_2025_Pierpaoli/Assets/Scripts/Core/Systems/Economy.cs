using Game.Core.Data;

namespace Game.Core.Systems
{
    public class Economy
    {
        readonly AppModel model; 
        readonly EconomyConfig cfg;

        public Economy(AppModel m, EconomyConfig c)
        {
            model = m; 
            cfg = c;
        }

        public bool TryBuy(int price)
        {
            if (model.coins < price) 
                return false; 
            model.coins -= price; 
            SaveSystem.Save(model); 
            return true;
        }
        public void GiveCoinReward()
        {
            model.coins += cfg.rewardPerCoin;
        
            SaveSystem.Save(model);
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
    }
}