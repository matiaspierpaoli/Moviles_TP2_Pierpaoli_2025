using Game.Core.ServicesCore;
using Game.UI.Views;
using UnityEngine;
using System;

namespace Game.Controller.Gameplay
{
    public class TutorialSequencer
    {
        private GameplayState host;
        private LevelController ctrl;
        private GameplayView view;
        private IInputStrategy input;
        private TutorialStepTrigger[] triggers;
        
        private int step = 0;
        private bool readyToRotate = false;

        public Action OnTutorialComplete;

        public TutorialSequencer(GameplayState host, LevelController ctrl, GameplayView view, IInputStrategy input, TutorialStepTrigger[] triggers)
        {
            this.host = host;
            this.ctrl = ctrl;
            this.view = view;
            this.input = input;
            this.triggers = triggers;
        }

        public void Start()
        {
            step = 0;
            
            view.tutorialClickInterceptor.gameObject.SetActive(true);
            view.tutorialClickInterceptor.onClick.AddListener(() => OnTutorialAction("next_step"));
            
            if (view.pointerTargetMenu) view.pointerTargetMenu.gameObject.SetActive(false);
            if (view.pointerTargetReset);view.pointerTargetReset.gameObject.SetActive(false);
            if (view.pointerTargetCalibrate);view.pointerTargetCalibrate.gameObject.SetActive(false);
            if (view.pointerTargetCoins) view.pointerTargetCoins.gameObject.SetActive(false);
            
            if (view.menuButton) view.menuButton.onClick.AddListener(() => OnTutorialAction("menu_button"));
            if (view.resetButton) view.resetButton.onClick.AddListener(() => OnTutorialAction("reset_button"));
            if (view.calibrateButton) view.calibrateButton.onClick.AddListener(() => OnTutorialAction("calibrate_button"));
            
            foreach (var trigger in triggers)
            {
                trigger.OnTutorialTrigger = (id) => OnTutorialAction(id);
            }
            
            ShowStep(step);
        }

        private void AdvanceStep()
        {
            step++;
            ShowStep(step);
        }
        
        public void Tick()
        {
            if (readyToRotate)
                ctrl.Tick(0f);
        }

        public void OnTutorialAction(string id)
        {
            switch (step)
            {
                case 0: // "Tap to Start"
                    if (id == "next_step") {
                        host.StartGameplay_Internal(); 
                        AdvanceStep();
                        readyToRotate = true;
                    }
                    break;
                case 1: // "Rotate to the sides"
                    if (id == "roll_target") { 
                        AdvanceStep();
                    }
                    break;
                case 2: // "Rotate Upwards/Backwards"
                    if (id == "pitch_target") {
                        AdvanceStep();
                        readyToRotate = false;
                        ctrl.ResetTilt();
                    }
                    break;
                case 3: // "Return to menu"
                    if (id == "next_step") { AdvanceStep(); }
                    break;
                case 4: // "Reset level"
                    if (id == "next_step") { AdvanceStep(); }
                    break;
                case 5: // "Recalibrate"
                    if (id == "next_step") { AdvanceStep(); }
                    break;
                case 6: // "Coins"
                    if (id == "next_step") { AdvanceStep(); }
                    break;
                case 7: // "Exit"
                    if (id == "next_step") { AdvanceStep(); readyToRotate = true; }
                    break;
                case 8:
                    if (id == "goal_target") { AdvanceStep(); }
                    break;
            }
        }

        private void ShowStep(int newStep)
        {
            switch (newStep)
            {
                case 0:
                    view.ShowTutorialStep("¡Welcome! Tap to start.", true);
                    break;
                case 1:
                    view.ShowTutorialStep("¡Great! Now rotate to the sides.");
                    break;
                case 2: 
                    view.ShowTutorialStep("¡Perfect! Now rotate upwards and backwards.");
                    break;
                case 3:
                    view.ShowTutorialStep("This button will let you go back tothe menu.", true);
                    if (view.menuButton) view.pointerTargetMenu.gameObject.SetActive(true);
                    break;
                case 4:
                    view.ShowTutorialStep("This resets the level if you feel stuck.",true);
                    if (view.resetButton) view.pointerTargetReset.gameObject.SetActive(true);
                    break;
                case 5: 
                    view.ShowTutorialStep("Use this one if rotation feels weird.", true);
                    if (view.calibrateButton) view.pointerTargetCalibrate.gameObject.SetActive(true);
                    break;
                case 6: 
                    view.ShowTutorialStep("Pick up coins to buy skins. This is your session counter.", true);
                    if (view.coinCountText) view.pointerTargetCoins.gameObject.SetActive(true);
                    break;
                case 7:
                    view.ShowTutorialStep("Get the ball to the exit. ¡That´s it!", true);
                    break;
                case 8:
                    view.ShowTutorialStep("");
                    break;
                case 9: 
                    view.HideTutorial();
                    CleanUp();
                    OnTutorialComplete?.Invoke();
                    break;
            }
        }
        
        public void CleanUp()
        {
            view.tutorialClickInterceptor.onClick.RemoveAllListeners();
            if (view.menuButton) view.menuButton.onClick.RemoveAllListeners();
            if (view.resetButton) view.resetButton.onClick.RemoveAllListeners();
            if (view.calibrateButton) view.calibrateButton.onClick.RemoveAllListeners();
            
            foreach (var trigger in triggers)
            {
                trigger.OnTutorialTrigger = null;
            }
        }
    }
}