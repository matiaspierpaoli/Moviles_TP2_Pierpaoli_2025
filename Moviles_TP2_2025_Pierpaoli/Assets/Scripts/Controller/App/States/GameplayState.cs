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
        LevelController controller;
        BaseGameplaySubstate sub;

        GameObject currentBoardInstance;

        public GameplayState(AppController a, ScreenView v, AppModel m) : base(a, v){ model=m; }

        public override void Enter(){
            base.Enter();
            var ld = AssetLoader.LoadLevel(model.currentLevel);
            var dc = AssetLoader.LoadDifficultyCurve();
            var ec = AssetLoader.LoadEconomy();

            
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
            }
            
            diff  = new DifficultyService(dc); 
            econ  = new Economy(model, ec);
            
            controller = new LevelController(new LevelModel(ld), boardView, app.inputStrategy, app.haptics, diff); 
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
            app.Go<MainMenuState>();
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
    }
}