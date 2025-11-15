using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level")]
    public class LevelData : ScriptableObject
    {
        public int id = 1;
        public int sessionSeconds = 240;
        
        public GameObject boardPrefab;

        public LevelParameters parameters;
        public TextAsset layoutJson;
        public Sprite backgroundSprite;
    }
}