using System;
using UnityEngine;

namespace Game.UI.Views
{
    public class CreditsView : ScreenView
    {
        [SerializeField] private float scrollSpeed = 40;
        [SerializeField] private RectTransform creditsContainer;
        
        public Action OnCreditsFinished;
        
        private Vector2 _initialPosition;
        private bool isScrolling = false;

        public override void Awake()
        {
            base.Awake();
            
            if (creditsContainer != null)
            {
                _initialPosition = creditsContainer.anchoredPosition;
            }
        }
        
        public void ResetPosition()
        {
            isScrolling = false;
            if (creditsContainer != null)
            {
                creditsContainer.anchoredPosition = _initialPosition;
            }
        }

        public void StartScrolling()
        {
            isScrolling = true;
        }

        public void StopScrolling()
        {
            isScrolling = false;
        }

        private void Update()
        {
            if (isScrolling && creditsContainer != null)
            {
                creditsContainer.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

                float containerBottom = creditsContainer.anchoredPosition.y; 
                
                if (creditsContainer.anchoredPosition.y > (Screen.height + creditsContainer.rect.height / 2))
                {
                    OnCreditsFinished?.Invoke();
                    isScrolling = false;
                }
            }
        }
    }
}