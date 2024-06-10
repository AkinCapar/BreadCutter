using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using BreadCutter.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BreadCutter.Controllers
{
    public class BasketController : BaseController
    {
        private BasketView _basketView;
        
        #region Injection

        private LevelSettings _levelSettings;
        
        public BasketController(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings;
        }

        #endregion
        public override void Initialize()
        {
            _signalBus.Subscribe<BasketSpawnedSignal>(OnBasketSpawnedSignal);
            _signalBus.Subscribe<ConveyorReachedLevelThree>(OnConveyorReachedLevelThree);
        }

        private void OnBasketSpawnedSignal(BasketSpawnedSignal signal)
        {
            MoveLoadedBasket(signal.Basket).Forget();
        }
        
        private void OnConveyorReachedLevelThree()
        {
            _basketView.RevealAdditionalBaskets();
        }

        private async UniTask MoveLoadedBasket(BasketView basket)
        {
            await UniTask.WaitForSeconds(_levelSettings.BasketWaitTime);
            
            if (_basketView != null)
            {
                _basketView.MoveLoadedBasket(_levelSettings.BasketMoveDuration);
            }
            
            _basketView = basket;
            _basketView.MoveToCollectionSpot(_levelSettings.BasketMoveDuration);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<BasketSpawnedSignal>(OnBasketSpawnedSignal);
            _signalBus.Unsubscribe<ConveyorReachedLevelThree>(OnConveyorReachedLevelThree);
        }
    }
}
