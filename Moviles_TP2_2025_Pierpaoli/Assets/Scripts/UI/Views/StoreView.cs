using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Core.Data;

namespace Game.UI.Views
{
    public class StoreView : ScreenView
    {
        [Header("UI References")] public TextMeshProUGUI totalCoinsText;
        public Button buyHiddenLevelButton;
        public TextMeshProUGUI buyHiddenLevelButtonText;

        public List<ShopItemSlot> shopSlots;

        public void UpdateGeneralUI(int totalCoins, int hiddenLevelPrice, bool isHiddenLevelUnlocked)
        {
            totalCoinsText.text = totalCoins.ToString();
        
            if (isHiddenLevelUnlocked)
            {
                buyHiddenLevelButtonText.text = "Bought";
                buyHiddenLevelButton.interactable = false;
            }
            else if (totalCoins >= hiddenLevelPrice)
            {
                buyHiddenLevelButtonText.text = $"Buy Secret Level: ({hiddenLevelPrice})";
                buyHiddenLevelButton.interactable = true;
            }
            else
            {
                buyHiddenLevelButtonText.text = $"Unsufficient ({hiddenLevelPrice})";
                buyHiddenLevelButton.interactable = false;
            }
        }

        public void SetupMaterialButtons(
            List<BallMaterialConfig.MaterialItem> materialItems,
            int totalCoins,
            string selectedId,
            List<string> ownedIds,
            Action<string> onMaterialClicked)
        {
            for (int i = 0; i < shopSlots.Count; i++)
            {
                if (i < materialItems.Count)
                {
                    var item = materialItems[i];
                    var slot = shopSlots[i];

                    bool isOwned = ownedIds.Contains(item.id);
                    bool isSelected = (item.id == selectedId);
                    bool canAfford = (totalCoins >= item.price);

                    slot.Setup(item, isOwned, isSelected, canAfford);

                    slot.button.onClick.RemoveAllListeners();
                    slot.button.onClick.AddListener(() => onMaterialClicked(item.id));
                }
                else
                {
                    shopSlots[i].Hide();
                }
            }
        }
    }
}
