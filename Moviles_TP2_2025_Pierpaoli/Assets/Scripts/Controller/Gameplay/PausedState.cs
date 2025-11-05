namespace Game.Core.GameplayModel
{
    public class PausedState : BaseGameplaySubstate
    {
        public PausedState(IGameplayHost h):base(h){}
        public override void Enter(){ UnityEngine.Time.timeScale=0f; }
        public override void Exit(){ UnityEngine.Time.timeScale=1f; }
    }
    public class WinState  : BaseGameplaySubstate { public WinState(IGameplayHost h):base(h){} public override void Enter(){ host.OnWin(); } }
    public class LoseState : BaseGameplaySubstate { public LoseState(IGameplayHost h):base(h){} public override void Enter(){ host.OnLose(); } }
}