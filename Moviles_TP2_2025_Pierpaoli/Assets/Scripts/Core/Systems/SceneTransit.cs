using Game.Core.Data;
using System; // <-- Añadido

namespace Game.Core.Systems
{
    public static class SceneTransit
    {
        public static Type NextState { get; private set; } 
        public static LoadingProfile Profile { get; private set; }

        public static void SetNext(Type nextState, LoadingProfile profile)
        {
            NextState = nextState;
            Profile = profile;
        }

        public static void Clear()
        {
            NextState = null;
            Profile = null;
        }
    }
}