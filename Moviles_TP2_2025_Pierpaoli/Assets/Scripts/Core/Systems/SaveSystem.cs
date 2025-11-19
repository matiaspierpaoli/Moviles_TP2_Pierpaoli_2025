using Game.Core.Data;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Core.Systems
{
    [System.Serializable]
    public class MaterialIdsWrapper {
        public List<string> ids;
    }
    public static class SaveSystem
    {
        const string K="save_v1";
        public static void Save(AppModel m){
            PlayerPrefs.SetInt($"{K}_coins", m.coins);
            PlayerPrefs.SetInt($"{K}_hidden", m.hiddenLevelUnlocked ? 1 : 0);
            PlayerPrefs.SetInt($"{K}_lvl", m.maxUnlockedLevel);
            PlayerPrefs.SetString($"{K}_selectedMat", m.selectedBallMaterialId);
            MaterialIdsWrapper wrapper = new MaterialIdsWrapper { ids = m.ownedBallMaterialIds };
            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString($"{K}_ownedMats", json);
            PlayerPrefs.SetInt($"{K}_tutorial", m.hasSeenTutorial ? 1 : 0);
            PlayerPrefs.SetInt($"{K}_firstOpen", m.hasUnlockedFirstOpen ? 1 : 0);
            PlayerPrefs.SetInt($"{K}_totalCoins", m.totalCoinsCollected);
            // ---------------------
            PlayerPrefs.Save();
        }
        public static void Load(AppModel m){
            m.coins       = PlayerPrefs.GetInt($"{K}_coins",0);
            m.hiddenLevelUnlocked = PlayerPrefs.GetInt($"{K}_hidden", 0) == 1;
            m.maxUnlockedLevel = PlayerPrefs.GetInt($"{K}_lvl", 1);
            m.selectedBallMaterialId = PlayerPrefs.GetString($"{K}_selectedMat", "Default"); 
            string json = PlayerPrefs.GetString($"{K}_ownedMats", "");
            if (!string.IsNullOrEmpty(json)) {
                MaterialIdsWrapper wrapper = JsonUtility.FromJson<MaterialIdsWrapper>(json);
                m.ownedBallMaterialIds = wrapper.ids;
            } else {
                m.ownedBallMaterialIds.Clear();
                m.ownedBallMaterialIds.Add("Default");
            }
            // ---------------------

            if (!m.ownedBallMaterialIds.Contains("Default")) {
                m.ownedBallMaterialIds.Add("Default");
                if (string.IsNullOrEmpty(m.selectedBallMaterialId)) m.selectedBallMaterialId = "Default";
            }
            
            m.hasSeenTutorial = PlayerPrefs.GetInt($"{K}_tutorial", 0) == 1;
            m.hasUnlockedFirstOpen = PlayerPrefs.GetInt($"{K}_firstOpen", 0) == 1;
            m.totalCoinsCollected = PlayerPrefs.GetInt($"{K}_totalCoins", 0);
        }
        
        public static void ClearSave()
        {
            PlayerPrefs.DeleteKey($"{K}_lvl");
            PlayerPrefs.DeleteKey($"{K}_coins");
            PlayerPrefs.DeleteKey($"{K}_hidden");
            PlayerPrefs.DeleteKey($"{K}_lvl");
            PlayerPrefs.DeleteKey($"{K}_selectedMat");
            PlayerPrefs.DeleteKey($"{K}_ownedMats");
            PlayerPrefs.DeleteKey($"{K}_tutorial");
            PlayerPrefs.DeleteKey($"{K}_firstOpen");
            PlayerPrefs.DeleteKey($"{K}_totalCoins");
            PlayerPrefs.Save();
        }
    }
}