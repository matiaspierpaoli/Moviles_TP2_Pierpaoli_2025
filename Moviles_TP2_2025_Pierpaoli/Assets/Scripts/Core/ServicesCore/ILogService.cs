namespace Game.Core.ServicesCore
{
    public interface ILogService
    {
        void Send(string msg, string stackTrace, string type);
        string GetAll();
        void ConfirmAndDelete(string unityReceiverGO, string callbackMethod);
    }

    // Editor/PC
    public class NoopLogService : ILogService
    {
        public void Send(string m, string s, string t) {}
        public string GetAll() => "";
        public void ConfirmAndDelete(string go, string cb) {}
    }
}