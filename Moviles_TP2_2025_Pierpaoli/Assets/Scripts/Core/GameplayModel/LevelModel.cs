using Game.Core.Data;

namespace Game.Core.GameplayModel
{
    public class LevelModel
    {
        public readonly LevelData data;

        public LevelModel(LevelData d)
        {
            data = d;
        }
    }
}