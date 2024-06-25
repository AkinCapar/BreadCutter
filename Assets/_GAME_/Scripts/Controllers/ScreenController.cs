using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using BreadCutter.Utils;
using BreadCutter.Views;
using UnityEngine;
using Zenject;

namespace BreadCutter.Controllers
{
    public class ScreenController : BaseController
    {
        private ScreenStates _currentState;
        private IdleClickerScreenView _idleClickerScreenView;
        
        #region Injection

        private DiContainer _diContainer;
        private PrefabSettings _prefabSettings;
        private CoinGainedFXView.Factory _coinGainedFXViewFactory;
        private Camera _mainCamera;
        
        public ScreenController(DiContainer diContainer
            , PrefabSettings prefabSettings
            , CoinGainedFXView.Factory coinGainedFXViewFactory
            , [Inject(Id = Constants.ZenjectIDs.Camera)] Camera camera)
        {
            _diContainer = diContainer;
            _prefabSettings = prefabSettings;
            _coinGainedFXViewFactory = coinGainedFXViewFactory;
            _mainCamera = camera;
        }
        #endregion
        
        public override void Initialize()
        {
            CreateState(ScreenStates.IdleClickerState);
            _signalBus.Subscribe<SliceSignal>(OnSliceSignal);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<SliceSignal>(OnSliceSignal);
        }

        public void ChangeState(ScreenStates state)
        {
            CreateState(state);
        }

        private void CreateState(ScreenStates state)
        {
            ClearScreens();
            _currentState = state;

            switch (_currentState)
            {
                case ScreenStates.IdleClickerState:
                    SwitchToIdleClickerScreen();
                    break;
            }
        }

        private void SwitchToIdleClickerScreen()
        {
            _idleClickerScreenView = 
                _diContainer.InstantiatePrefabForComponent<IdleClickerScreenView>(_prefabSettings.IdleClickerScreenPrefab);
            
            _idleClickerScreenView.Initialize();
        }

        private void OnSliceSignal(SliceSignal signal)
        {
            CoinGainedFXView coinGainedFXView = _coinGainedFXViewFactory.Create();
            coinGainedFXView.transform.SetParent(_idleClickerScreenView.transform);
            coinGainedFXView.transform.position = _mainCamera.WorldToScreenPoint(signal.SlicePosition);
            coinGainedFXView.SetTheEarnedAmount(signal.SliceObject.pricePerSlice);
        }

        private void ClearScreens()
        {
            
        }
    }
}
