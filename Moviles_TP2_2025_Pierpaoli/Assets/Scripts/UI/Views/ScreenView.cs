using UnityEngine;
using System.Collections;

namespace Game.UI.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenView : MonoBehaviour, IScreenView
    {
        CanvasGroup cg;

        public virtual void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }

        public void Hide()
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
            gameObject.SetActive(false);
        }

        public IEnumerator Fade(float to, float d)
        {
            var a0 = cg.alpha;
            var t = 0f;
            gameObject.SetActive(true);
            while (t < d)
            {
                t += Time.unscaledDeltaTime;
                cg.alpha = Mathf.Lerp(a0, to, t / d);
                yield return null;
            }

            cg.alpha = to;
            if (to <= 0) Hide();
            else
            {
                cg.blocksRaycasts = true;
                cg.interactable = true;
            }
        }
    }
}