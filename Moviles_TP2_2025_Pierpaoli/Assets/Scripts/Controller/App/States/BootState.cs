using UnityEngine;
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
            //base.Enter();
            //app.logs.Send("Boot", "", "Info");
            
            if (app.model.hasSeenTutorial)
            {
                SceneTransit.SetNext(typeof(MainMenuState), app.bootProfile);
            }
            else
            {
                app.model.startTutorial = true;
                app.model.SetLevel(1); 

                SceneTransit.SetNext(typeof(GameplayState), app.levelProfile);
            }
            app.Go<LoadingState>();
        }
    }
}