using TMPro;

namespace Game.UI.Views
{
    public class VictoryView: ScreenView
    {
        public TextMeshProUGUI sessionCoinsText;
        public TextMeshProUGUI totalCoinsText;

        public void ShowResults(int sessionCoins, int totalCoins)
        {
            if (sessionCoinsText != null)
            {
                sessionCoinsText.text = "Coins earned: " + sessionCoins;
            }

            if (totalCoinsText != null)
            {
                totalCoinsText.text = "Total coins: " + totalCoins;
            }
        }
    }
}