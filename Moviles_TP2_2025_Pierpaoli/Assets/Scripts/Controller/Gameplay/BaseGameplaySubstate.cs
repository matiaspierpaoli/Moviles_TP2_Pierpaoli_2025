namespace Game.Core.GameplayModel
{
    public abstract class BaseGameplaySubstate
    {
        protected readonly IGameplayHost host;
        protected BaseGameplaySubstate(IGameplayHost h){ host = h; }
        public virtual void Enter(){} public virtual void Exit(){} public virtual void Tick(){}
    }
}