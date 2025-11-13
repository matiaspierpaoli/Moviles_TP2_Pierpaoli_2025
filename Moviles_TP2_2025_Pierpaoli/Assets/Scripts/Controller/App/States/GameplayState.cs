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
        
        public GameplayState(AppController a, ScreenView v, AppModel m) : base(a, v){ model=m; }

        public override void Enter(){
            base.Enter();
            
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Screen.orientation !=  ScreenOrientation.LandscapeLeft)
                Screen.orientation = ScreenOrientation.LandscapeLeft;
#endif
            
            if (app.smartInput != null)
            {
                app.smartInput.CalibrateToCurrent();
            }
            
            var ld = AssetLoader.LoadLevel(model.currentLevel);
            var dc = AssetLoader.LoadDifficultyCurve();
            var ec = AssetLoader.LoadEconomy();
            
            ballMaterialCfg = AssetLoader.LoadBallMaterialConfig();
            
            diff  = new DifficultyService(dc); 
            econ  = new Economy(model, ec, ballMaterialCfg);
            
            var boardPrefab = Resources.Load<GameObject>($"Prefabs/Level/Level_{model.currentLevel}");
            
            BoardView boardView = null;
            if (boardPrefab != null)
            {
                currentBoardInstance = Object.Instantiate(boardPrefab);
                
                boardView = currentBoardInstance.GetComponent<BoardView>();
            }
            
            if (boardView == null)
            {
                Debug.LogError($"Error al cargar el prefab del nivel {model.currentLevel}. Asegurate de que exista en 'Resources/Prefabs/' y tenga un componente 'BoardView'.");
                app.Go<LevelSelectState>();
                return;
            }
            
            if (currentBoardInstance != null)
            {
                GoalTrigger goal = currentBoardInstance.GetComponentInChildren<GoalTrigger>();

                if (goal != null)
                {
                    goal.OnGoalReached = this.OnWin;
                }
                else
                {
                    Debug.LogError("¡No se encontro 'GoalTrigger.cs' en el prefab del nivel!");
                }
                
                CoinSpawner spawner = currentBoardInstance.GetComponentInChildren<CoinSpawner>();
                if (spawner != null)
                {
                    spawner.OnCoinSpawned = (CoinPickup coin) => 
                    {
                        coin.OnCollected = HandleCoinCollected;
                    };

                    spawner.SpawnCoins();
                }
            }
            
            BallMaterialConfig.MaterialItem selectedItem = econ.GetSelectedMaterialItem();
            if (selectedItem != null && selectedItem.material != null)
            {
                MeshRenderer ballRenderer = boardView.ball.GetComponent<MeshRenderer>();
                if (ballRenderer != null)
                {
                    ballRenderer.material = selectedItem.material;
                }
            }
            
            controller = new LevelController(new LevelModel(ld), boardView, app.inputStrategy, app.haptics, diff); 
            controller.StartLevel(); 
            
            sub = new PlayingState(this, controller); 
            sub.Enter(); 
            
            gameplayUI = view as GameplayView;
            sessionCoins = 0;
            gameplayUI?.UpdateCoinCount(sessionCoins);
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