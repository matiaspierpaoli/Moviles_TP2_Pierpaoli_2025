namespace Game.Core.ServicesCore
{
    public interface IGoogleService
    {
        void Init();
        void Authenticate(System.Action<string> cb = null);
        bool IsAuthenticated { get; }
        void Increment(string id, int steps = 1);
        void Unlock(string id);
        void ShowUI();
        void LogEvent(string name);
    }
}