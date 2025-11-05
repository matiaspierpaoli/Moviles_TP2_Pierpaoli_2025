using Game.Core.ServicesCore;

namespace Game.External
{
    public class AndroidLogService : ILogService
    {
        public void Send(string m, string st, string t) => AndroidLogPlugin.Send(m, st, t);
        public string GetAll() => AndroidLogPlugin.GetAll();
        public void ConfirmAndDelete(string go, string cb) => AndroidLogPlugin.ConfirmAndDelete(go, cb);
    }
}