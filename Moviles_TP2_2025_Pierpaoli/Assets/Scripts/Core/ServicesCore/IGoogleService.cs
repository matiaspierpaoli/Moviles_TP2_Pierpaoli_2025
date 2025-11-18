namespace Game.Core.ServicesCore
{
    public interface IGoogleService
    {
        void Init();
        bool Ready { get; }
        void Increment(string id, int steps = 1);
        void Unlock(string id);
        void ShowUI();
        void LogEvent(string name);
    }
}