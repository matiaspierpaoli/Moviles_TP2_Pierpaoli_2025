using Game.Core.Data;
using UnityEngine;

namespace Game.Core.Systems
{
    public static class AssetLoader
    {
        static LevelData cur;

        public static LevelData LoadLevel(int id)
        {
            cur = Resources.Load<LevelData>($"Levels/Level{id}");
            return cur;
        }

        public static void UnloadLevel()
        {
            cur = null;
            Resources.UnloadUnusedAssets();
        }

        public static LevelParameters LoadDifficultyCurve() =>
            Resources.Load<LevelParameters>("Difficulty/DifficultyCurve");

        public static EconomyConfig LoadEconomy() => Resources.Load<EconomyConfig>("Economy/EconomyConfig");
    }
}