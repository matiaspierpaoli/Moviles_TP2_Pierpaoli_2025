using Game.UI.Views;
using UnityEngine;

namespace Game.Controller
{
    public class LogViewState : BaseState
    {
        private LogView logView;

        public LogViewState(AppController a, ScreenView v) : base(a, v)
        {
            logView = v as LogView;
        }

        public override void Enter()
        {
            base.Enter();
            if (logView == null) return;

            logView.refreshButton.onClick.AddListener(Refresh);
            logView.deleteButton.onClick.AddListener(TapDelete);

            logView.OnLogsDeleted = Refresh;

            Refresh();
        }

        public override void Exit()
        {
            if (logView != null)
            {
                logView.refreshButton.onClick.RemoveListener(Refresh);
                logView.deleteButton.onClick.RemoveListener(TapDelete);
                logView.OnLogsDeleted = null;
            }

            base.Exit();
        }

        private void Refresh()
        {
            if (logView == null) return;
            var s = app.logs.GetAll();
            logView.logText.text = string.IsNullOrEmpty(s) ? "<i>No logs to display</i>" : s;
        }

        private void TapDelete()
        {
            if (logView == null) return;
            app.logs.ConfirmAndDelete(logView.gameObject.name, nameof(logView.OnDeleteResult));
        }
    }
}