using Game.UI.Views;
namespace Game.Controller
{
    public class BootState : BaseState
    {
        public BootState(AppController a, ScreenView v):base(a,v){}
        public override void Enter()
        {
            base.Enter();
            app.google.Init();
            app.logs.Send("Boot", "", "Info");
            app.Go<MainMenuState>();
        }
    }
}