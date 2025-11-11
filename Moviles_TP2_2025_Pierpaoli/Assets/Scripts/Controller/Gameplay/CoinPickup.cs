using UnityEngine;
using System;

namespace Game.Controller.Gameplay
{
    public class CoinPickup: MonoBehaviour
    {
        public Action OnCollected;
        [SerializeField] private string playerTag = "Player";
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                Debug.Log("Collected");
                OnCollected?.Invoke();
                gameObject.SetActive(false); 
            }
        }
    }
}