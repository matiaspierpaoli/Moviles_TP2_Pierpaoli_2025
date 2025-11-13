using UnityEngine;
using System.Collections.Generic;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "BallMaterialConfig", menuName = "Game/Ball Material Config")]
    public class BallMaterialConfig : ScriptableObject
    {
        [System.Serializable]
        public class MaterialItem
        {
            public string id;
            public string displayName;
            public Material material;
            public Sprite icon;
            public int price; 
            public bool isDefault; 
        }

        public List<MaterialItem> materialItems;
    }
}