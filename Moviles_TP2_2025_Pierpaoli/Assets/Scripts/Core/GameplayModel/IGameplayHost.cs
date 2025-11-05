namespace Game.Core.GameplayModel
{
    public interface IGameplayHost
    {
        void OnWin();
        void OnLose();
        void ToPaused();
        void ToPlaying();
    }
}