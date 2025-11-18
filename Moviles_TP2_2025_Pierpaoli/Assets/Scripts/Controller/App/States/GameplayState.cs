using System.Collections;
using Game.Controller.Gameplay;
using Game.Core.GameplayModel;
using Game.Core.Data;
using Game.Core.Systems;
using Game.UI.Views;
using UnityEngine;

namespace Game.Controller
{
    public class GameplayState : BaseState, IGameplayHost
    {
        readonly AppModel model;
        DifficultyService diff;
        Economy econ;
        BallMaterialConfig ballMaterialCfg;
        LevelController controller;
        BaseGameplaySubstate sub;

        GameObject currentBoardInstance;

        private GameplayView gameplayUI;
        private int sessionCoins;
        private CoinSpawner activeSpawner;
        
        private TutorialSequencer tutorial;
        public GameplayState(AppController a, ScreenView v, AppModel m) : base(a, v){ model=m; }

        public override void Enter()
        {
            base.Enter();
            
            gameplayUI = view as GameplayView;
            sessionCoins = 0;
            gameplayUI?.UpdateCoinCount(sessionCoins);
            gameplayUI?.HideTutorial();

            var ld = AssetLoader.LoadLevel(model.currentLevel);
            var ec = AssetLoader.LoadEconomy();
            
            if (app.backgroundImage != null && ld.backgroundSprite != null)
            {
                app.backgroundImage.sprite = ld.backgroundSprite;
                app.backgroundImage.enabled = true;
            }
            
            ballMaterialCfg = AssetLoader.LoadBallMaterialConfig();
            econ  = new Economy(model, ec, ballMaterialCfg);

            var boardPrefab = ld.boardPrefab;

            BoardView boardView = null;
            if (boardPrefab != null)
            {
                currentBoardInstance = Object.Instantiate(boardPrefab);
                boardView = currentBoardInstance.GetComponent<BoardView>();
                
                if (boardView != null && boardView.ball != null)
                {
                    var selectedItem = econ.GetSelectedMaterialItem();
                    if (selectedItem != null) boardView.ball.GetComponent<MeshRenderer>().material = selectedItem.material;
                }
            }

            if (boardView == null) { app.Go<LevelSelectState>(); return; }

            GoalTrigger goal = currentBoardInstance.GetComponentInChildren<GoalTrigger>();
            if (goal) goal.OnGoalReached = this.OnWin;

            activeSpawner = currentBoardInstance.GetComponentInChildren<CoinSpawner>();
            if (activeSpawner)
            {
                activeSpawner.OnCoinSpawned = (coin) => coin.OnCollected = HandleCoinCollected;
            }

            diff = new DifficultyService(ld.parameters);
            controller = new LevelController(new LevelModel(ld), boardView, app.inputStrategy, app.haptics, diff);

            if (model.startTutorial)
            {
                
            
                gameplayUI.readyPanel.gameObject.SetActive(false);
                
                var tutorialTriggers = currentBoardInstance.GetComponentsInChildren<TutorialStepTrigger>(true);
            
                StartTutorial(tutorialTriggers);
            }
            else
            {
                gameplayUI?.ShowReadyPhase(StartGameplay);
            }
        }

        private void StartGameplay()
        {
            CalibrateInput();
            SpawnCoins();
            controller.StartLevel();
            sub = new PlayingState(this, controller);
            sub.Enter();
        }
        
        private void StartTutorial(TutorialStepTrigger[] triggers) 
        {
            tutorial = new TutorialSequencer(this, controller, gameplayUI, app.inputStrategy, triggers);
        
            tutorial.OnTutorialComplete = () => {
                tutorial = null;
                model.hasSeenTutorial = true; 
                SaveSystem.Save(model);
                app.Go<MainMenuState>();
            };
            tutorial.Start();
        }
        
        public void StartGameplay_Internal()
        {
            StartGameplay();
        }
        
        public override void Tick()
        {
            if (tutorial != null)
                tutorial.Tick();
            else
                sub?.Tick();
        }
        
        public override void Exit(){
            sub?.Exit();
            controller?.Dispose();

            if (app.backgroundImage != null)
            {
                app.backgroundImage.enabled = false;
            }
            
            if (currentBoardInstance != null)
            {
                Object.Destroy(currentBoardInstance);
            }
            
            AssetLoader.UnloadLevel();
            base.Exit();
        }

        public void CalibrateInput()
        {
            if (app.smartInput != null) {
                app.smartInput.CalibrateToCurrent();
                app.smartInput.ResetToCurrent();
            }
        }
        
        public void SpawnCoins()
        {
            if (activeSpawner != null) {
                activeSpawner.OnCoinSpawned = (coin) => coin.OnCollected = HandleCoinCollected;
                activeSpawner.SpawnCoins();
            }
        }
        
        public void OnWin()
        {
            if (app.model.startTutorial)
            {
                model.startTutorial = false;
                
                if (app.google.Ready)
                {
                    app.google.Unlock(GPGSIds.achievement_tutorial_completed);
                }
                else
                {
                    Debug.Log("Google is not ready to unlock/increment in gameplay state OnWin()");
                }
                
                app.Go<MainMenuState>();
            }
            else
            {
                econ.AddSessionCoins(sessionCoins);
                model.lastSessionCoins = sessionCoins;
                app.Go<VictoryState>();
            }
        }

        public void OnLose()
        {
            app.Go<MainMenuState>();
        }

        public void ToPaused()
        {
            sub?.Exit(); 
            sub = new PausedState(this); 
            sub.Enter();
        }

        public void ToPlaying()
        {
            sub?.Exit(); 
            sub = new PlayingState(this, controller); 
            sub.Enter();
        }
        
        private void HandleCoinCollected()
        {
            app.haptics.PlaySimpleVibration();
            sessionCoins++;
            gameplayUI?.UpdateCoinCount(sessionCoins);
        }
    }
}