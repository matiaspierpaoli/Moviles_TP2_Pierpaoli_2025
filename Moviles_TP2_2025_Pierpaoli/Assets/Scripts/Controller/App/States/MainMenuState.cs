using Game.UI.Views;
using Game.Core.Data;
using Game.Core.Systems;

namespace Game.Controller
{
    public class MainMenuState : BaseState
    {
        readonly AppModel model;
        public MainMenuState(AppController a, ScreenView v, AppModel m) : base(a, v)
        {
            model = m;
        }
        
        public override void Enter()
        {
            if (app.backgroundImage != null)
            {
                app.backgroundImage.enabled = false;
            }
        
            base.Enter();
            
            if (!app.model.hasSeenTutorial && !app.model.tutorialStarted)
            {
                StartTutorial();
            }
        }
        
        private void StartTutorial()
        {
            app.model.tutorialStarted = true;
            app.model.SetLevel(1);
            app.Go<GameplayState>();
        }
    }
}