using Game.Core.Data;

namespace Game.Core.Systems
{
    public class DifficultyService
    {
        readonly LevelParameters props;

        public DifficultyService(LevelParameters p)
        {
            props = p;
        }
        public int HoleCount() => props.holeCount;
        public float RotFriction() => props.ballFriction;
    }
}