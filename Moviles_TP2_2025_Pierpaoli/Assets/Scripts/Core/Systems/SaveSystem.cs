using Game.Core.Data;
using UnityEngine;

namespace Game.Core.Systems
{
    public static class SaveSystem
    {
        const string K="save_v1";
        public static void Save(AppModel m){
            PlayerPrefs.SetInt($"{K}_lvl", m.maxUnlocked);
            PlayerPrefs.SetInt($"{K}_coins", m.coins);
            PlayerPrefs.SetInt($"{K}_hidden", m.hiddenLevelUnlocked ? 1 : 0);
            PlayerPrefs.Save();
        }
        public static void Load(AppModel m){
            m.maxUnlocked = PlayerPrefs.GetInt($"{K}_lvl",1);
            m.coins       = PlayerPrefs.GetInt($"{K}_coins",0);
            m.hiddenLevelUnlocked = PlayerPrefs.GetInt($"{K}_hidden", 0) == 1;
        }
        
        public static void ClearSave()
        {
            PlayerPrefs.DeleteKey($"{K}_lvl");
            PlayerPrefs.DeleteKey($"{K}_coins");
            PlayerPrefs.DeleteKey($"{K}_hidden");
            PlayerPrefs.Save();
        }
    }
}