using Game.Core.GameplayModel;
using Game.Core.ServicesCore;
using Game.Core.Systems;

namespace Game.Controller.Gameplay
{
    public class LevelController
    {
        readonly LevelModel model;
        readonly BoardView view;
        readonly IInputStrategy input;
        readonly IHaptics haptics;
        readonly DifficultyService diff;

        public LevelController(LevelModel m, BoardView v, IInputStrategy i, IHaptics h, DifficultyService d)
        {
            model = m;
            view = v;
            input = i;
            haptics = h;
            diff = d;
        }

        public void StartLevel()
        {
            
        }

        public void Tick(float n01)
        {
            var tilt = input.ReadTilt();
            var max = diff.TiltMax(n01);
            view.SetBoardTilt(tilt, max);
        }

        public void OnHit()
        {
            haptics.Hit();
        }

        public void OnGoal()
        {
            haptics.Goal();
        }

        public void Dispose()
        {
            
        }
    }
}