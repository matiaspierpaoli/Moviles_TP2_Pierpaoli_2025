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
            PlayerPrefs.Save();
        }
        public static void Load(AppModel m){
            m.maxUnlocked = PlayerPrefs.GetInt($"{K}_lvl",1);
            m.coins       = PlayerPrefs.GetInt($"{K}_coins",0);
        }
    }
}