using UnityEngine;

namespace Game.Controller
{
    public class App : MonoBehaviour
    {
        public AppController controller;

        void Awake()
        {
            if (!controller) controller = FindObjectOfType<AppController>(true);
            if (controller) DontDestroyOnLoad(controller.gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}