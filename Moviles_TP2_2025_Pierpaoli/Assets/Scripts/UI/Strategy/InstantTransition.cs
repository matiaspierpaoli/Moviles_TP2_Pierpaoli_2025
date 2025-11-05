using System.Collections;
using Game.UI.Views;

namespace Game.UI.Strategy
{
    public class InstantTransition : ITransitionStrategy
    {
        public IEnumerator Play(ScreenView from, ScreenView to)
        {
            if (from) from.Hide();
            if (to) to.Show();
            yield break;
        }
    }
}