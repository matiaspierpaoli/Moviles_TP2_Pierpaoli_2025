using UnityEngine;

namespace Game.UI
{
    [ExecuteAlways]
    public class SafeAreaView : MonoBehaviour
    {
        Rect last;

        void OnEnable()
        {
            Apply();
        }

        void Update()
        {
            if (Screen.safeArea != last)
            {
                Apply();
            }
        }

        void Apply()
        {
            var rt = transform as RectTransform;
            if (!rt) return;
            last = Screen.safeArea;
            var a = last;
            var min = a.position;
            var max = a.position + a.size;
            min.x /= Screen.width;
            min.y /= Screen.height;
            max.x /= Screen.width;
            max.y /= Screen.height;
            rt.anchorMin = min;
            rt.anchorMax = max;
        }
    }
}