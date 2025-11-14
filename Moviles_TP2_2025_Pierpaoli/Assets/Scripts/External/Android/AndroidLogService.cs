using UnityEngine;
using Game.Core.ServicesCore;

namespace Game.External
{
    public class AndroidLogService : ILogService
    {
        public AndroidLogService()
        {
            Application.logMessageReceivedThreaded += OnLog;
            Debug.Log("AndroidLogService inicializado y capturando logs.");
            // --------------------
        }

        ~AndroidLogService()
        {
            Application.logMessageReceivedThreaded -= OnLog;
        }

        private void OnLog(string condition, string stackTrace, LogType type)
        {
            Send(condition, stackTrace, type.ToString());
        }

        public void Send(string m, string st, string t)
        {
            AndroidLogPlugin.Send(m, st, t);
        }

        public string GetAll()
        {
            return AndroidLogPlugin.GetAll();
        }

        public void ConfirmAndDelete(string go, string cb)
        {
            AndroidLogPlugin.ConfirmAndDelete(go, cb);
        }
    }
}