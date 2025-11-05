using Game.Core.Data;

namespace Game.Core.Systems
{
    public class DifficultyService
    {
        readonly DifficultyCurve curve;

        public DifficultyService(DifficultyCurve c)
        {
            curve = c;
        }

        public float TiltMax(float n01) => curve.TiltMax(n01);
        public int HoleCount(float n01) => curve.HoleCount(n01);
        public float RotFriction(float n01) => curve.RotFriction(n01);
    }
}