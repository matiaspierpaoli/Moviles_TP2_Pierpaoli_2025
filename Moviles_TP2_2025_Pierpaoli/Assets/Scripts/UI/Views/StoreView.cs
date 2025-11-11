using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game.UI.Views
{
    public class StoreView : ScreenView
    {
        [Header("UI References")]
        public TextMeshProUGUI totalCoinsText;
        public Button buyHiddenLevelButton;
        public TextMeshProUGUI buyHiddenLevelButtonText;
        public TextMeshProUGUI buyButtonText;

        public void UpdateView(int totalCoins, int price, bool isUnlocked)
        {
            totalCoinsText.text =  "$" +  totalCoins;
            buyHiddenLevelButtonText.text = "Level template";
            
            if (isUnlocked)
            {
                buyButtonText.text = "Bought";
                buyHiddenLevelButton.interactable = false;
            }
            else if (totalCoins >= price)
            {
                buyButtonText.text = "Buy: " + price;
                buyHiddenLevelButton.interactable = true;
            }
            else
            {
                buyButtonText.text = "Unsufficient";
                buyHiddenLevelButton.interactable = false;
            }
        }
    }
}