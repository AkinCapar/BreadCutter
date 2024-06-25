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
        private List<BasketView> _baskets;
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
            _signalBus.Subscribe<InitialBasketsSpawned>(OnInitialBasketsSpawnedSignal);
        }

        private void OnBasketSpawnedSignal(BasketSpawnedSignal signal)
        {
            MoveLoadedBasket(signal.Basket).Forget();
        }

        private void OnInitialBasketsSpawnedSignal(InitialBasketsSpawned signal)
        {
            _baskets = signal.Baskets;
            _basketView = _baskets[0];
            foreach (BasketView basket in _baskets)
            {
                basket.MoveForward(_levelSettings.BasketMoveDuration, 12);
            }
        }

        private async UniTask MoveLoadedBasket(BasketView basket)
        {
            if (_basketView == null)
            {
                return;
            }
            
            await UniTask.WaitForSeconds(_levelSettings.BasketWaitTime);
            

            _basketView.MoveLoadedBasket(_levelSettings.BasketMoveDuration);
            _baskets.Remove(_basketView);
            
            _basketView = _baskets[0];

            foreach (BasketView basketView in _baskets)
            {
                basketView.MoveForward(_levelSettings.BasketMoveDuration, 6);
            }
            
            _baskets.Add(basket);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<BasketSpawnedSignal>(OnBasketSpawnedSignal);
            _signalBus.Unsubscribe<InitialBasketsSpawned>(OnInitialBasketsSpawnedSignal);
        }
    }
}
