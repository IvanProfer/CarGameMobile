using Game.Car;
using Game.InputLogic;
using Game.TapeBackground;
using Profile;
using Tool;
using Services.Analytics;

namespace Game
{
    internal class GameController : BaseController
    {
        private AnalyticsManager _analytics;
        public GameController(ProfilePlayer profilePlayer)
        {
            
            var leftMoveDiff = new SubscriptionProperty<float>();
            var rightMoveDiff = new SubscriptionProperty<float>();

            var tapeBackgroundController = new TapeBackgroundController(leftMoveDiff, rightMoveDiff);
            AddController(tapeBackgroundController);

            var inputGameController = new InputGameController(leftMoveDiff, rightMoveDiff, profilePlayer.CurrentCar);
            AddController(inputGameController);

            var carController = new CarController();
            AddController(carController);
        }
    }
}
