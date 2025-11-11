using Game.UI.Views;
using Game.Core.Systems;

namespace Game.Controller
{
    public class BootState : BaseState
    {
        public BootState(AppController a, ScreenView v):base(a,v){}
        public override void Enter()
        {
            SaveSystem.Load(app.model);
            base.Enter();
            //app.logs.Send("Boot", "", "Info");
            app.Go<MainMenuState>();
        }
    }
}