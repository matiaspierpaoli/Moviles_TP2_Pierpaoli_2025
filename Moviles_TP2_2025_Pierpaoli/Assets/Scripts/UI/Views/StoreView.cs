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
        public TextMeshProUGUI buyButtonText;

        public List<ShopItemSlot> shopSlots;

        public void UpdateGeneralUI(int totalCoins, int hiddenLevelPrice, bool isHiddenLevelUnlocked)
        {
            totalCoinsText.text = totalCoins.ToString();
        
            // if (isHiddenLevelUnlocked)
            // {
            //     buyButtonText.text = "Comprado";
            //     buyHiddenLevelButton.interactable = false;
            // }
            // else if (totalCoins >= hiddenLevelPrice)
            // {
            //     buyButtonText.text = $"Comprar Nivel ({hiddenLevelPrice})";
            //     buyHiddenLevelButton.interactable = true;
            // }
            // else
            // {
            //     buyButtonText.text = $"Insuficiente ({hiddenLevelPrice})";
            //     buyHiddenLevelButton.interactable = false;
            // }
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
