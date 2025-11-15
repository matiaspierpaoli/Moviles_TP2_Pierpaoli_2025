using UnityEngine;
using Game.Core.Systems;
using Game.UI.Views;
using System;

namespace Game.Controller
{
    public class LoadingState : BaseState
    {
        private LoadingController loadingController;
        private LoadingView loadingView;
        public LoadingState(AppController a, ScreenView v) : base(a, v)
        {
            loadingView = v as LoadingView;
        
            if (loadingView != null)
            {
                loadingController = new LoadingController(
                    loadingView.progressBar,
                    loadingView.backgoundImage,
                    loadingView.hintParent
                );
            }
            else
            {
                Debug.LogError("La 'LoadingView' asignada en AppController no tiene el script 'LoadingView.cs'");
            }
            
            if (app.backgroundImage != null)
            {
                app.backgroundImage.enabled = false;
            }
        }

        public override void Enter()
        {
            base.Enter();
            if (loadingController == null)
            {
                OnLoadComplete();
                return;
            }
            app.StartCoroutine(loadingController.RunFakeLoad(OnLoadComplete));
        }

        private void OnLoadComplete()
        {
            Type nextState = SceneTransit.NextState;

            if (nextState != null)
            {
                app.fsm.Change(nextState);
            }
            else
            {
                app.Go<MainMenuState>();
            }
        }
    }
}