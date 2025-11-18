using Game.Core.ServicesCore;

namespace Game.External
{
    public class DummyGpgsService : IGoogleService
    {
        public void Init() { }
        public bool Ready { get; }
        public void Authenticate(System.Action<string> cb = null){ cb?.Invoke("Editor"); }
        public bool IsAuthenticated => false;
        public void Increment(string id, int steps = 1) { }
        public void Unlock(string id) { }
        public void ShowUI() { }
        public void LogEvent(string name) { }
    }
}