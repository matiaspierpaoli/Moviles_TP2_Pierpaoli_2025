using TMPro;

namespace Game.UI.Views
{
    public class GameplayView : ScreenView
    {
        public TextMeshProUGUI coinCountText;

        public void UpdateCoinCount(int count)
        {
            if (coinCountText != null)
            {
                coinCountText.text = "Coins: " + count.ToString();
            }
        }
    }
}