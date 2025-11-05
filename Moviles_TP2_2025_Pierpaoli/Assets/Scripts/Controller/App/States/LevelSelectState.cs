using Game.Core.Data;
using Game.UI.Views;
namespace Game.Controller
{
    public class LevelSelectState : BaseState
    {
        readonly AppModel model;

        public LevelSelectState(AppController a, ScreenView v, AppModel m) : base(a, v)
        {
            model = m;
        }
    }
}