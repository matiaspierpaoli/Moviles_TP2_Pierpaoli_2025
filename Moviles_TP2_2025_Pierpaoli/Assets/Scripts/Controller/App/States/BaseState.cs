using Game.UI.Views;
namespace Game.Controller
{
    public abstract class BaseState : IAppState
    {
        protected readonly AppController app;
        protected readonly ScreenView view;

        protected BaseState(AppController a, ScreenView v)
        {
            app = a;
            view = v;
        }

        public virtual void Enter()
        {
            app.Swap(null, view);
        }
    
        public virtual void Tick()
        {
            
        }
        public virtual void Exit()
        {
            view?.Hide();
        }
    }

}