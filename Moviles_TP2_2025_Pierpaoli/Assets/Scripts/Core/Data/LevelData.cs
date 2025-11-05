using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level")]
    public class LevelData : ScriptableObject
    {
        public int id = 1;
        public int sessionSeconds = 240; // 4 min
        public float gravityBase = 9.8f;
        public float friction = 0.2f;
        public TextAsset layoutJson;
    }
}