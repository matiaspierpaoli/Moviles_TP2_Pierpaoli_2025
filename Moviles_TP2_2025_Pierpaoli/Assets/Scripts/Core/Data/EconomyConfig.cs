using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "Game/Economy")]
    public class EconomyConfig : ScriptableObject
    {
        public int rewardPerLevel = 25;
        public int rewardPerCoin = 1;
        public int itemPrice = 50;
        public int hiddenLevelPrice = 5;
    }
}