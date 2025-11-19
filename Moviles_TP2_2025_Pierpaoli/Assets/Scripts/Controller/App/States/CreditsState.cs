using Game.UI.Views;

namespace Game.Controller
{
    public class CreditsState : BaseState
    {
        private CreditsView creditsView;
        public CreditsState(AppController a, ScreenView v) : base(a, v)
        {
            creditsView = v as CreditsView;
        }

        public override void Enter()
        {
            base.Enter();
            
            if (creditsView != null)
            {
                creditsView.OnCreditsFinished = OnCreditsComplete;
                
                creditsView.StartScrolling();
            }
            else
            {
                OnCreditsComplete();
            }
        }
        
        public override void Exit()
        {
            if (creditsView != null)
            {
                creditsView.OnCreditsFinished = null;
            
                creditsView.ResetPosition(); 
            }
            base.Exit();
        }

        private void OnCreditsComplete()
        {
            app.Go<MainMenuState>();
        }
    }
}