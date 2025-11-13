using UnityEngine;
using System.Collections.Generic;

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
        
        public string selectedBallMaterialId; 
        public List<string> ownedBallMaterialIds = new List<string>();
        public void SetLevel(int lvl)
        {
            var cl = Mathf.Clamp(lvl, 1, 3);
            currentLevel = cl;
            if (cl > maxUnlocked) maxUnlocked = cl;
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
            maxUnlocked = 1;
            coins = 0;
            hiddenLevelUnlocked = false;
            lastSessionCoins = 0;
            selectedBallMaterialId = "1";
            ownedBallMaterialIds.Clear();
            ownedBallMaterialIds.Add("1");
        }
    }
}