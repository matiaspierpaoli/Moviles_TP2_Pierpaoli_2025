using Game.Core.GameplayModel;
using Game.Core.ServicesCore;
using Game.Core.Systems;
using UnityEngine;

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

        public void ResetTilt()
        {
            view.ResetBoardToDefaultState();
        }
        
        public void Tick(float n01)
        {
            var tilt = input.ReadTilt();
            view.SetBoardTilt(tilt);
        }

        public void Dispose()
        {
            
        }
    }
}