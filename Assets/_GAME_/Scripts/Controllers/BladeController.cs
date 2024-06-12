using System.Collections;
using System.Collections.Generic;
using BreadCutter.Data;
using BreadCutter.Models;
using BreadCutter.Settings;
using BreadCutter.Utils;
using BreadCutter.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BreadCutter.Controllers
{
    public class BladeController : BaseController, ITickable
    {
        private BladeView _blade;

        #region Injection

        private LevelSettings _levelSettings;
        private BladeSettings _bladeSettings;
        private UpgradeController _upgradeController;

        public BladeController(LevelSettings levelSettings
            , BladeSettings bladeSettings
            , UpgradeController upgradeController)
        {
            _levelSettings = levelSettings;
            _bladeSettings = bladeSettings;
            _upgradeController = upgradeController;
        }

        #endregion

        public override void Initialize()
        {
            _signalBus.Subscribe<BladeSpawnedSignal>(OnBladeSpawnedSignal);
            _signalBus.Subscribe<BasketIsLoadedSignal>(OnBasketIsLoadedSignal);
            _signalBus.Subscribe<BasketSpawnedSignal>(OnBasketSpawnedSignal);
            _signalBus.Subscribe<SlicingBreadDespawnedSignal>(OnSlicingBreadDespawned);
            _signalBus.Subscribe<PlayerCooldownSignal>(OnPlayerCooldownSignal);
            _signalBus.Subscribe<UpgradeBladeButtonPressedSignal>(OnUpgradeBladeButtonPressedSignal);
        }

        private void OnBladeSpawnedSignal(BladeSpawnedSignal signal)
        {
            _blade = signal.Blade;
        }

        private void OnBasketIsLoadedSignal()
        {
            _blade.WaitForBasketChange();
        }

        private void OnBasketSpawnedSignal()
        {
            BladeCanMove().Forget();
        }

        private void OnSlicingBreadDespawned()
        {
            _blade.SlicingBreadDespawned();
        }

        private void OnPlayerCooldownSignal(PlayerCooldownSignal signal)
        {
            float remapValue = 0 + (signal.TimeHeldDown - 0) * (0.2f * _levelSettings.PlayerInputEffectAmount - 0) /
                (_levelSettings.MaxCooldownAmount - 0);
            _blade.SetInputPower(1 + remapValue);
        }

        private async UniTask BladeCanMove()
        {
            await UniTask.WaitForSeconds(_levelSettings.BladeWaitTime);
            _blade.BladeCanMove();
        }

        private void OnUpgradeBladeButtonPressedSignal()
        {
            if (_blade.BladeLevel + 1 < _bladeSettings.BladeData.Length && _upgradeController.SpendCoinForUpgrade(UpgradeTypes.BladeUpgrade))
            {
                BladeData newData = _bladeSettings.BladeData[_blade.BladeLevel + 1];
                _blade.UpgradeBlade(newData);
                _signalBus.Fire<BladeUpgradedSignal>();
            }
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<BladeSpawnedSignal>(OnBladeSpawnedSignal);
            _signalBus.Unsubscribe<BasketIsLoadedSignal>(OnBasketIsLoadedSignal);
            _signalBus.Unsubscribe<BasketSpawnedSignal>(OnBasketSpawnedSignal);
            _signalBus.Unsubscribe<SlicingBreadDespawnedSignal>(OnSlicingBreadDespawned);
            _signalBus.Unsubscribe<PlayerCooldownSignal>(OnPlayerCooldownSignal);
            _signalBus.Unsubscribe<UpgradeBladeButtonPressedSignal>(OnUpgradeBladeButtonPressedSignal);
        }

        public void Tick()
        {
            if (_blade != null)
            {
                _blade.MoveBlade(Vector3.back * _levelSettings.BladeInitialSpeed);
                _blade.TurnBlade(_levelSettings.BladeTurnSpeed);
            }
        }
    }
}