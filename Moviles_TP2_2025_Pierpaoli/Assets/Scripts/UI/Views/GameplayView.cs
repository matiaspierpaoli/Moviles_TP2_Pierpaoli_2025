using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Game.UI.Views
{
    public class GameplayView : ScreenView
    {
        [Header("HUD Elements")]
        public TextMeshProUGUI coinCountText;

        [Header("Ready Panel Elements")]
        public GameObject readyPanel; 
        public Button startButton;

        public void UpdateCoinCount(int count)
        {
            if (coinCountText != null) coinCountText.text = count.ToString();
        }

        public void ShowReadyPhase(Action onStartClicked)
        {
            if (readyPanel) readyPanel.SetActive(true);
        
            if (startButton)
            {
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(() => {
                    onStartClicked?.Invoke();
                    if (readyPanel) readyPanel.SetActive(false);
                });
            }
        }
    }
}