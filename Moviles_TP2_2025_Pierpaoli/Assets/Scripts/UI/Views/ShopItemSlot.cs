using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core.Data;

namespace Game.UI.Views
{
    public class ShopItemSlot : MonoBehaviour
    {
        [Header("Referencias UI")]
        public Button button;
        public Image iconImage;
        public TextMeshProUGUI labelText;
    
        [HideInInspector] public string currentMaterialId;

        public void Setup(BallMaterialConfig.MaterialItem item, bool isOwned, bool isSelected, bool canAfford)
        {
            currentMaterialId = item.id;
        
            if (iconImage != null) iconImage.sprite = item.icon;

            if (isOwned)
            {
                if (isSelected)
                {
                    labelText.text = "Equiped";
                    button.interactable = false; 
                }
                else
                {
                    labelText.text = "Equip";
                    button.interactable = true;
                }
            }
            else
            {
                labelText.text = "Price:  " + item.price.ToString();
                button.interactable = canAfford;
            }
        
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}