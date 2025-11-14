using UnityEngine;
using System;

namespace Game.Controller.Gameplay
{
    public class TutorialStepTrigger : MonoBehaviour
    {
        public string stepID;
    
        public Action<string> OnTutorialTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnTutorialTrigger?.Invoke(stepID);
                gameObject.SetActive(false); 
            }
        }
    }
}