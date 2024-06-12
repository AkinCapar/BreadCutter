using BreadCutter.Settings;
using BreadCutter.Utils;
using BreadCutter.Views;

namespace BreadCutter.Controllers
{
    public class ConveyorController : BaseController
    {
        private ConveyorView _conveyorView;

        #region Injection

        private LevelSettings _levelSettings;
        private UpgradeController _upgradeController;

        public ConveyorController(LevelSettings levelSettings
            , UpgradeController upgradeController)
        {
            _levelSettings = levelSettings;
            _upgradeController = upgradeController;
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
            if (_conveyorView.ConveyorLevel < _levelSettings.MaxConveyorLevel &&
                _upgradeController.SpendCoinForUpgrade(UpgradeTypes.Expand))
            {
                _conveyorView.Expand();
                _signalBus.Fire<ConveyorExpandedSignal>();
            }
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
            _signalBus.Unsubscribe<ExpandButtonPressedSignal>(OnExpandButtonPressedSignal);
        }
    }
}