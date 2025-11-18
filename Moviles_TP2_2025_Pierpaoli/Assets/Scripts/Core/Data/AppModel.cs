using UnityEngine;
using System.Collections.Generic;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "AppModel", menuName = "Game/AppModel")]
    public class AppModel : ScriptableObject
    {
        public int maxLevels = 5;
        public int currentLevel = 1;
        public int maxUnlockedLevel = 1;
        public int coins = 0;
        public bool hiddenLevelUnlocked = false;
        public bool hasSeenTutorial = false;
        public bool hasUnlockedFirstOpen = false;

        [System.NonSerialized]
        public int lastSessionCoins;
        [System.NonSerialized]
        public bool startTutorial = false;
        
        public string selectedBallMaterialId; 
        public List<string> ownedBallMaterialIds = new List<string>();
        public void SetLevel(int lvl)
        {
            currentLevel = Mathf.Clamp(lvl, 1, maxLevels);
        }
        
        public void AddOwnedMaterial(string materialId)
        {
            if (!ownedBallMaterialIds.Contains(materialId))
            {
                ownedBallMaterialIds.Add(materialId);
            }
        }
        public void ResetToDefaults()
        {
            currentLevel = 1;
            maxUnlockedLevel = 1;
            coins = 0;
            hiddenLevelUnlocked = false;
            hasUnlockedFirstOpen = false;
            lastSessionCoins = 0;
            selectedBallMaterialId = "1";
            ownedBallMaterialIds.Clear();
            ownedBallMaterialIds.Add("1");
        }
    }
}