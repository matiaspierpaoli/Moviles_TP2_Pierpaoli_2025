using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class LevelSelectView : ScreenView
    {
        [System.Serializable]
        public class LevelButton
        {
            public int levelIndex; 
            public Button button;
            public GameObject lockIcon;
        }
    
        public Button tutorialButton;
        public LevelButton[] levelButtons;
    }
}