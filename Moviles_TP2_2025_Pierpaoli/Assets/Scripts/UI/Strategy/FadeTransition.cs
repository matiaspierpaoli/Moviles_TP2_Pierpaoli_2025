using System.Collections;
using Game.UI.Views;

namespace Game.UI.Strategy
{
    public class FadeTransition : ITransitionStrategy
    {
        readonly float d;

        public FadeTransition(float dur = 0.25f)
        {
            d = dur;
        }

        public IEnumerator Play(ScreenView from, ScreenView to)
        {
            if (to)
            {
                to.gameObject.SetActive(true);
                var cg = to.GetComponent<UnityEngine.CanvasGroup>();
                if (cg) cg.alpha = 0;
            }

            if (from !=null) yield return from.Fade(0, d);
            if (to !=null) yield return to.Fade(1, d);
        }
    }
}