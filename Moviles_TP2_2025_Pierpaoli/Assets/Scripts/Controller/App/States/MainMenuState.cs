using Game.UI.Views;
using Game.Core.Systems;

namespace Game.Controller
{
    public class MainMenuState : BaseState
    {
        public MainMenuState(AppController a, ScreenView v) : base(a, v)
        {
        }
        
        public override void Enter()
        {
            if (app.backgroundImage != null)
            {
                app.backgroundImage.enabled = false;
            }
        
            base.Enter();
            
            if (!app.model.hasSeenTutorial)
            {
                StartTutorial();
            }
        }
        
        private void StartTutorial()
        {
            app.model.startTutorial = true;
            app.model.SetLevel(1);
            app.Go<GameplayState>();
        }
    }
}