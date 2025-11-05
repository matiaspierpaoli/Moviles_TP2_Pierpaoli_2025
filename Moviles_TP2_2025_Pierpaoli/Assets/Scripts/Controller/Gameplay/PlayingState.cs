using Game.Core.GameplayModel;
using Game.Core.Systems;
namespace Game.Controller.Gameplay
{
    public class PlayingState : BaseGameplaySubstate
    {
        readonly LevelController ctrl; readonly SessionTimer timer;
        public PlayingState(IGameplayHost h, LevelController c, SessionTimer t):base(h){ ctrl=c; timer=t; }
        public override void Enter(){ timer.Reset(); }
        public override void Tick(){
            timer.Tick(); ctrl.Tick(timer.Normalized);
            if (timer.IsOver) host.OnWin();
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Escape)) host.ToPaused();
        }
    }
}