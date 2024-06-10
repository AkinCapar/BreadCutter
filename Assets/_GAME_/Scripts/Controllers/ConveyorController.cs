using BreadCutter.Views;
namespace BreadCutter.Controllers
{
    public class ConveyorController : BaseController
    {
        private ConveyorView _conveyorView;
        
        #region Injection

        public ConveyorController()
        {
            
        }
        #endregion
        
        public override void Initialize()
        {
            _signalBus.Subscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
            _signalBus.Subscribe<ExpandButtonPressedSignal>(OnExpandButtonPressedSignal);
        }

        private void OnLevelSpawnedSignal(LevelSpawnedSignal signal)
        {
            _conveyorView = signal.LevelView.conveyorView;
        }

        private void OnExpandButtonPressedSignal()
        {
            _conveyorView.Expand();
            _signalBus.Fire<ConveyorExpandedSignal>();
        }
        
        public override void Dispose()
        {
            _signalBus.Unsubscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
            _signalBus.Unsubscribe<ExpandButtonPressedSignal>(OnExpandButtonPressedSignal);
        }
    }
}
