using UnityEngine;

namespace Game.UI.Views
{
    public class TutorialView : MonoBehaviour
    {
        public GameObject panel;

        public void ShowOnce()
        {
            var seen = PlayerPrefs.GetInt("tutorial_seen", 0) == 1;
            if (!seen)
            {
                panel.SetActive(true);
                PlayerPrefs.SetInt("tutorial_seen", 1);
            }
        }

        public void Close()
        {
            panel.SetActive(false);
        }
    }
}