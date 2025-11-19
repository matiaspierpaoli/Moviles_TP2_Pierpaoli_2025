using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "Game/Economy")]
    public class EconomyConfig : ScriptableObject
    {
        public int rewardPerCoin = 1;
        public int hiddenLevelPrice = 5;
    }
}