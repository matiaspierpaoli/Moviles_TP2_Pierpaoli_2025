using Game.Core.ServicesCore;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace Game.External
{
    public class AndroidGpgsService : IGoogleService
    {
        bool ready;

        public void Init()
        {
#if UNITY_ANDROID
            if (ready) return;

            PlayGamesPlatform.DebugLogEnabled = false;
            PlayGamesPlatform.Activate();
            ready = true;
#endif
        }

        public void Authenticate(System.Action<string> cb = null)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.Authenticate(status =>
            {
                cb?.Invoke(status.ToString());
            });
#else
            cb?.Invoke("Editor");
#endif
        }

        public bool IsAuthenticated =>
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.localUser.authenticated;
#else
            false;
#endif

        public void Increment(string id, int steps = 1)
        {
#if UNITY_ANDROID
            if (!IsAuthenticated) return;
            PlayGamesPlatform.Instance.IncrementAchievement(id, steps, _ => { });
#endif
        }

        public void Unlock(string id)
        {
#if UNITY_ANDROID
            if (!IsAuthenticated) return;
            PlayGamesPlatform.Instance.UnlockAchievement(id, _ => { });
#endif
        }

        public void ShowUI()
        {
#if UNITY_ANDROID
            if (!IsAuthenticated) return;
            PlayGamesPlatform.Instance.ShowAchievementsUI();
#endif
        }

        public void LogEvent(string name) { }
    }
}