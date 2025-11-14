using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Game.UI.Views

{
    public class LogView : ScreenView
    {
        [Header("UI References")]
        public TextMeshProUGUI logText;
        public Button refreshButton;
        public Button deleteButton;

        public Action OnLogsDeleted;

        public void OnDeleteResult(string result)
        {
            if (result == "deleted")
            {
                OnLogsDeleted?.Invoke();
            }
        }
    }
}