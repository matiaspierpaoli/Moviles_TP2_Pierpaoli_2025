using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Game.Core.Data;

namespace Game.Controller
{
    public class BootLoader : MonoBehaviour
    {
        [Header("Addressables")] [SerializeField]
        private string mainSceneAddress = "MainScene";

        [Header("Loading Profile")] [SerializeField]
        private LoadingProfile bootProfile;

        [Header("UI References")] [SerializeField]
        private Slider progressBar;

        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject hintParent;
        [SerializeField] private CanvasGroup loadingCanvas;

        void Start()
        {
            ConfigureLoadingUI();
            StartCoroutine(LoadMainSceneWithProfile());
        }

        private void ConfigureLoadingUI()
        {
            if (bootProfile == null) return;

            if (backgroundImage != null && bootProfile.backgoundSprite != null)
            {
                backgroundImage.sprite = bootProfile.backgoundSprite;
                backgroundImage.enabled = true;
            }

            if (hintParent != null)
                hintParent.SetActive(bootProfile.shouldShowHint);

            if (progressBar != null)
                progressBar.gameObject.SetActive(bootProfile.showProgressBar);
        }

        private IEnumerator LoadMainSceneWithProfile()
        {
            float elapsed = 0f;
            bool minimumTimeReached = false;

            while (!minimumTimeReached)
            {
                elapsed += Time.deltaTime;

                float normalizedTime = Mathf.Clamp01(elapsed / bootProfile.minDisplayTime);
                float visualProgress = bootProfile.fakeCurve.Evaluate(normalizedTime);

                if (progressBar != null)
                    progressBar.value = visualProgress;

                if (elapsed >= bootProfile.minDisplayTime)
                    minimumTimeReached = true;

                yield return null;
            }

            AsyncOperationHandle<SceneInstance> handle =
                Addressables.LoadSceneAsync(mainSceneAddress, LoadSceneMode.Single);
            
            if (progressBar != null)
                progressBar.value = 1f;

            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}