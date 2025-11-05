using System.Collections;
using Game.UI.Views;

namespace Game.UI.Strategy
{
    public interface ITransitionStrategy
    {
        IEnumerator Play(ScreenView from, ScreenView to);
    }
}