using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class LoadingView : ScreenView
    {
        [Header("Loading Components")]
        public Slider progressBar;
        public Image backgoundImage;
        public GameObject hintParent;
    }
}