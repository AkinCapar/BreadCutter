using System.Collections;
using System.Collections.Generic;
using BreadCutter.Models;
using BreadCutter.Settings;
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
        private BladeModel _bladeModel;

        public BladeController(LevelSettings levelSettings
            , BladeModel bladeModel)
        {
            _levelSettings = levelSettings;
            _bladeModel = bladeModel;
        }

        #endregion
        
        public override void Initialize()
        {
            _signalBus.Subscribe<BladeSpawnedSignal>(OnBladeSpawnedSignal);
            _signalBus.Subscribe<BasketIsLoadedSignal>(OnBasketIsLoadedSignal);
            _signalBus.Subscribe<BasketSpawnedSignal>(OnBasketSpawnedSignal);
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

        private async UniTask BladeCanMove()
        {
            await UniTask.WaitForSeconds(_levelSettings.BladeWaitTime);
            _blade.BladeCanMove();
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<BladeSpawnedSignal>(OnBladeSpawnedSignal);
            _signalBus.Unsubscribe<BasketIsLoadedSignal>(OnBasketIsLoadedSignal);
            _signalBus.Unsubscribe<BasketSpawnedSignal>(OnBasketSpawnedSignal);
        }

        public void Tick()
        {
            if (_blade != null)
            {
                _blade.MoveBlade(Vector3.back * _levelSettings.BladeInitialSpeed);
            }
        }
    }
}
