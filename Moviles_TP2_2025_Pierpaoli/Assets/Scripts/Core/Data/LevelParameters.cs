using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "LevelParameters", menuName = "Game/Level Parameters")]
    public class LevelParameters : ScriptableObject
    {
        [Header("Propiedades del Nivel")]
        public int holeCount = 4;
        public float ballFriction = 0.2f; 
        // Later on: sphere speed, gravity, etc.
    }
}