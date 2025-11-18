using UnityEngine;
using Game.Core.Data;
using Game.UI.Views;
namespace Game.Controller
{
    public class LevelSelectState : BaseState
    {
        readonly AppModel model;
        private LevelSelectView levelView;

        public LevelSelectState(AppController a, ScreenView v, AppModel m) : base(a, v)
        {
            model = m;
            levelView = v as LevelSelectView;
        }
        
        public override void Enter()
        {
            base.Enter();
            if (levelView == null) return;
            
            if (app.backgroundImage != null)
            {
                app.backgroundImage.enabled = false;
            }
            
            levelView.tutorialButton.onClick.RemoveAllListeners();
            levelView.tutorialButton.onClick.AddListener(app.Ui_StartTutorial);

            foreach (var lb in levelView.levelButtons)
            {
                bool isUnlocked = (lb.levelIndex <= model.maxUnlockedLevel);
            
                lb.button.interactable = isUnlocked;
                if (lb.lockIcon != null) lb.lockIcon.SetActive(!isUnlocked);
            
                lb.button.onClick.RemoveAllListeners();
            
                if (isUnlocked)
                {
                    int levelToPlay = lb.levelIndex; 
                    lb.button.onClick.AddListener(() => app.Ui_PlayLevel(levelToPlay));
                }
            }
        }
    }
}