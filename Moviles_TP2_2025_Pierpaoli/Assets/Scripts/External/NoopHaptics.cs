using Game.Core.ServicesCore;
namespace Game.External
{
    public class NoopHaptics : IHaptics
    {
        public void Hit()
        {
        }

        public void Goal()
        {
        }
    }
}