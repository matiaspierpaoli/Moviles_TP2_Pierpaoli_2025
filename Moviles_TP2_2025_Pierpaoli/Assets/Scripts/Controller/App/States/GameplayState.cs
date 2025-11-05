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
        SessionTimer timer;
        DifficultyService diff;
        Economy econ;
        LevelController controller;
        BaseGameplaySubstate sub;

        public GameplayState(AppController a, ScreenView v, AppModel m) : base(a, v){ model=m; }

        public override void Enter(){
            base.Enter();
            var ld = AssetLoader.LoadLevel(model.currentLevel);
            var dc = AssetLoader.LoadDifficultyCurve();
            var ec = AssetLoader.LoadEconomy();

            timer = new SessionTimer(ld.sessionSeconds);
            diff  = new DifficultyService(dc);
            econ  = new Economy(model, ec);
            controller = new LevelController(new LevelModel(ld), Object.FindAnyObjectByType<BoardView>(), app.inputStrategy, app.haptics, diff);

            controller.StartLevel();
            sub = new PlayingState(this, controller, timer);
            sub.Enter();
        }

        public override void Exit(){
            sub?.Exit(); controller?.Dispose();
            AssetLoader.UnloadLevel(); base.Exit();
        }

        // IGameplayHost
        public void OnWin(){ econ.GiveLevelReward(); model.SetLevel(model.currentLevel+1); app.Go<LevelSelectState>(); }
        public void OnLose(){ app.Go<LevelSelectState>(); }
        public void ToPaused(){ sub?.Exit(); sub = new PausedState(this); sub.Enter(); }
        public void ToPlaying(){ sub?.Exit(); sub = new PlayingState(this, controller, timer); sub.Enter(); }
    }
}