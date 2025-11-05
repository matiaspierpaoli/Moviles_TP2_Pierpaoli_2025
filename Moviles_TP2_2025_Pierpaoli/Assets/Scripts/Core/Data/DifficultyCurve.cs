using UnityEngine;

namespace Game.Core.Data
{
    [CreateAssetMenu(fileName = "DifficultyCurve", menuName = "Game/Difficulty")]
    public class DifficultyCurve : ScriptableObject
    {
        public AnimationCurve tiltMax = AnimationCurve.Linear(0, 10, 1, 20);
        public AnimationCurve holeCount = AnimationCurve.Linear(0, 2, 1, 6);
        public AnimationCurve rotFriction = AnimationCurve.Linear(0, 0.2f, 1, 0.35f);

        public float TiltMax(float t) => tiltMax.Evaluate(t);
        public int HoleCount(float t) => Mathf.RoundToInt(holeCount.Evaluate(t));
        public float RotFriction(float t) => rotFriction.Evaluate(t);
    }
}