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
        
        public GameplayState(AppController a, ScreenView v, AppModel m) : base(a, v){ model=m; }

        public override void Enter()
        {
            base.Enter();
            
            gameplayUI = view as GameplayView;
            sessionCoins = 0;
            gameplayUI?.UpdateCoinCount(sessionCoins);

            var ld = AssetLoader.LoadLevel(model.currentLevel);
            var dc = AssetLoader.LoadDifficultyCurve();
            var ec = AssetLoader.LoadEconomy();
            
            ballMaterialCfg = AssetLoader.LoadBallMaterialConfig();
            econ  = new Economy(model, ec, ballMaterialCfg);

            var boardPrefab = Resources.Load<GameObject>($"Prefabs/Level/Level_{model.currentLevel}");
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

            diff  = new DifficultyService(dc);
            controller = new LevelController(new LevelModel(ld), boardView, app.inputStrategy, app.haptics, diff);

            gameplayUI?.ShowReadyPhase(StartGameplay);
        }

        private void StartGameplay()
        {
            if (app.smartInput != null)
            {
                app.smartInput.CalibrateToCurrent();
                app.smartInput.ResetToCurrent();
            }

            if (activeSpawner != null)
            {
                activeSpawner.SpawnCoins();
            }

            controller.StartLevel();
            sub = new PlayingState(this, controller);
            sub.Enter();
        }
        
        public override void Tick()
        {
            sub?.Tick();
        }
        
        public override void Exit(){
            sub?.Exit();
            controller?.Dispose();

            if (currentBoardInstance != null)
            {
                Object.Destroy(currentBoardInstance);
            }
            
            AssetLoader.UnloadLevel();
            base.Exit();
        }

        public void OnWin()
        {
            econ.AddSessionCoins(sessionCoins);
            model.lastSessionCoins = sessionCoins;

            app.Go<VictoryState>();
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