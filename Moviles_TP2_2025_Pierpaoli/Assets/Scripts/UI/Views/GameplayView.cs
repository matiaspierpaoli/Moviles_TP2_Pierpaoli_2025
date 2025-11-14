using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Game.UI.Views
{
    public class GameplayView : ScreenView
    {
        [Header("HUD Elements")]
        public TMP_Text coinCountText;
        public Button menuButton;
        public Button resetButton;
        public Button calibrateButton;

        [Header("Ready Panel Elements")]
        public GameObject readyPanel; 
        public Button startButton;
        
        [Header("Tutorial Elements")]
        public GameObject tutorialPanel; 
        public TextMeshProUGUI tutorialText;
        public Button tutorialClickInterceptor;
        
        public Image pointerTargetMenu;
        public Image pointerTargetReset;
        public Image pointerTargetCalibrate;
        public Image pointerTargetCoins;

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
        
        public void ShowTutorialStep(string text, bool showClickInterceptor = false)
        {
            if (tutorialPanel) tutorialPanel.SetActive(true);
            
            if (pointerTargetMenu) pointerTargetMenu.gameObject.SetActive(false);
            if (pointerTargetReset);pointerTargetReset.gameObject.SetActive(false);
            if (pointerTargetCalibrate);pointerTargetCalibrate.gameObject.SetActive(false);
            if (pointerTargetCoins) pointerTargetCoins.gameObject.SetActive(false);
            
            if (tutorialText) tutorialText.text = text;
    
            if (tutorialClickInterceptor) tutorialClickInterceptor.gameObject.SetActive(showClickInterceptor);
        }

        public void HideTutorial()
        {
            if (tutorialPanel) tutorialPanel.SetActive(false);
            if (tutorialClickInterceptor) tutorialClickInterceptor.gameObject.SetActive(false);
        }
    }
}