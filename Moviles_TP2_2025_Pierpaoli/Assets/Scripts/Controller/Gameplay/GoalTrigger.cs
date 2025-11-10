using UnityEngine;
using System;

namespace Game.Controller.Gameplay
{
    public class GoalTrigger : MonoBehaviour
    {
        public Action OnGoalReached;
        [SerializeField] private string playerTag = "Player";
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                OnGoalReached?.Invoke();
            }
        }
    }
}