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
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Screen.orientation !=  ScreenOrientation.Portrait)
                Screen.orientation = ScreenOrientation.Portrait;
#endif
            
            SaveSystem.Load(app.model);
            //base.Enter();
            //app.logs.Send("Boot", "", "Info");
            SceneTransit.SetNext(typeof(MainMenuState), app.bootProfile);
            app.Go<LoadingState>();
        }
    }
}