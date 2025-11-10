using Game.Core.GameplayModel;

namespace Game.Controller.Gameplay
{
    public class PlayingState : BaseGameplaySubstate
    {
        readonly LevelController ctrl; 
        public PlayingState(IGameplayHost h, LevelController c):base(h)
        { 
            ctrl = c; 
        }

        public override void Enter()
        {
        }
        public override void Tick()
        {
            ctrl.Tick(0f);
        }
    }
}