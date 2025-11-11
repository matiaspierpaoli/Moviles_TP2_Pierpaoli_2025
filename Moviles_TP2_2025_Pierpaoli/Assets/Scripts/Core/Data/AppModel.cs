using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "AppModel", menuName = "Game/AppModel")]
    public class AppModel : ScriptableObject
    {
        [Range(1, 3)] public int currentLevel = 1;
        public int maxUnlocked = 1;
        public int coins = 0;
        public bool hiddenLevelUnlocked = false;

        [System.NonSerialized]
        public int lastSessionCoins;
        public void SetLevel(int lvl)
        {
            var cl = Mathf.Clamp(lvl, 1, 3);
            currentLevel = cl;
            if (cl > maxUnlocked) maxUnlocked = cl;
        }
        
        public void ResetToDefaults()
        {
            currentLevel = 1;
            maxUnlocked = 1;
            coins = 0;
            hiddenLevelUnlocked = false;
            lastSessionCoins = 0;
        }
    }
}