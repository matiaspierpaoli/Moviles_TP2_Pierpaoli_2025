using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game.Core.Data;
using Game.Core.Systems;
using System;

namespace Game.UI.Views
{
    public class LoadingController
    {
        Slider progressBar;
        Image backgoundImage;
        GameObject hintParent;
        
        LoadingProfile profile;
        float minDisplayTime;
        AnimationCurve fakeCurve;

        public LoadingController(Slider pb, Image bg, GameObject hint)
        {
            progressBar = pb;
            backgoundImage = bg;
            hintParent = hint;
        }
        
        public IEnumerator RunFakeLoad(Action onComplete)
        {
            profile = SceneTransit.Profile;
            if (profile)
            {
                minDisplayTime = profile.minDisplayTime;
                fakeCurve = profile.fakeCurve;
                if (backgoundImage) backgoundImage.sprite = profile.backgoundSprite;
                if (hintParent) hintParent.SetActive(profile.shouldShowHint);
                if (progressBar) progressBar.gameObject.SetActive(profile.showProgressBar);
            }

            float visual = 0f, elapsed = 0f;
            while (true)
            {
                elapsed += Time.unscaledDeltaTime;
                
                float t = Mathf.Clamp01(elapsed / minDisplayTime);
                float fake = fakeCurve.Evaluate(t);

                visual = Mathf.MoveTowards(visual, fake, 2.5f * Time.unscaledDeltaTime);
                if (progressBar) progressBar.value = visual;

                if (t >= 1f) break; 
                yield return null;
            }
            
            yield return new WaitForSecondsRealtime(0.2f);

            onComplete?.Invoke();
            SceneTransit.Clear();
        }
    }
}