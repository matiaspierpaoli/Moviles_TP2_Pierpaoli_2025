using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Controller.Gameplay
{
    public class CoinSpawner: MonoBehaviour
    {
        public GameObject coinPrefab;
        public List<Transform> spawnPoints;
        public int coinsToSpawn = 5;

        public Action<CoinPickup> OnCoinSpawned;

        public void SpawnCoins()
        {
            if (coinPrefab == null || spawnPoints.Count == 0) return;

            List<Transform> availablePoints = new List<Transform>(spawnPoints);
            int amountToSpawn = Mathf.Min(coinsToSpawn, availablePoints.Count);
            
            for (int i = 0; i < amountToSpawn; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availablePoints.Count);
                Transform spawnPoint = availablePoints[randomIndex];

                GameObject coinInstance = Instantiate(coinPrefab, spawnPoint.position, spawnPoint.rotation);
                coinInstance.transform.SetParent(this.transform);

                availablePoints.RemoveAt(randomIndex);

                CoinPickup pickup = coinInstance.GetComponent<CoinPickup>();
                if (pickup != null)
                {
                    OnCoinSpawned?.Invoke(pickup);
                }
            }
        }
    }
}