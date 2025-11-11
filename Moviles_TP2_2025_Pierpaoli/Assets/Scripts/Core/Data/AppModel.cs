using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "AppModel", menuName = "Game/AppModel")]
    public class AppModel : ScriptableObject
    {
        [Range(1, 3)] public int currentLevel = 1;
        public int maxUnlocked = 1;
        public int coins = 0;

        [System.NonSerialized]
        public int lastSessionCoins;
        public void SetLevel(int lvl)
        {
            var cl = Mathf.Clamp(lvl, 1, 3);
            currentLevel = cl;
            if (cl > maxUnlocked) maxUnlocked = cl;
        }
    }
}