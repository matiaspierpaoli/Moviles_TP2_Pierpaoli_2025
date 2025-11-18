using UnityEngine;
using Game.Core.ServicesCore;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace Game.External
{
    public class AndroidGpgsService : IGoogleService
    {
        private bool isAuthenticated = false;
        private bool ready = false;
        public bool Ready => ready;

        public void Init()
        {
#if UNITY_ANDROID
            if (ready) return;

            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Instance.Authenticate(OnSignInResult);
#endif
        }
#if UNITY_ANDROID
        private void OnSignInResult(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                Debug.Log("Google Play Games sign-in successful.");
                ready = true;
            }
            else
            {
                Debug.Log("Google Play Games sign-in failed: " + status);
                ready = false;
            }
        }
#endif
        public bool IsAuthenticated =>
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.localUser.authenticated;
#else
            false;
#endif

        public void Increment(string id, int steps = 1)
        {
#if UNITY_ANDROID
            if (!ready)
            {
                UnityEngine.Debug.Log("GPGS: Intento de 'Increment' fallido. El usuario NO está autenticado.");
                return; 
            }
            
            UnityEngine.Debug.Log($"GPGS: Incrementando logro '{id}' en {steps} pasos.");
            PlayGamesPlatform.Instance.IncrementAchievement(id, steps, (bool success) => {
                if(success)
                    UnityEngine.Debug.Log($"GPGS: Logro '{id}' incrementado con éxito en el servidor.");
                else
                    UnityEngine.Debug.Log($"GPGS: El incremento del logro '{id}' falló en el servidor.");
            });
#endif
        }

        public void Unlock(string id)
        {
#if UNITY_ANDROID
            // --- LOG AÑADIDO ---
            if (!ready)
            {
                UnityEngine.Debug.LogWarning("GPGS: Intento de 'Unlock' fallido. El usuario NO está autenticado.");
                return;
            }
            // -------------------
            
            UnityEngine.Debug.Log($"GPGS: Desbloqueando logro '{id}'.");
            PlayGamesPlatform.Instance.UnlockAchievement(id, _ => { });
#endif
        }

        public void ShowUI()
        {
#if UNITY_ANDROID
            // --- LOG AÑADIDO ---
            if (!IsAuthenticated)
            {
                UnityEngine.Debug.LogWarning("GPGS: Intento de 'ShowUI' fallido. El usuario NO está autenticado.");
                return;
            }
            // -------------------
            
            PlayGamesPlatform.Instance.ShowAchievementsUI();
#endif
        }

        public void LogEvent(string name) { }
    }
}