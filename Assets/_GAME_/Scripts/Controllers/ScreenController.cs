using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using BreadCutter.Utils;
using UnityEngine;
using Zenject;

namespace BreadCutter.Controllers
{
    public class ScreenController : BaseController
    {
        private ScreenStates _currentState;
        
        #region Injection

        private DiContainer _diContainer;
        private PrefabSettings _prefabSettings;
        
        public ScreenController(DiContainer diContainer
            , PrefabSettings prefabSettings)
        {
            _diContainer = diContainer;
            _prefabSettings = prefabSettings;
        }
        #endregion
        
        public override void Initialize()
        {
            CreateState(ScreenStates.IdleClickerState);
        }

        public override void Dispose()
        {
            
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
            IdleClickerScreenView screenView = 
                _diContainer.InstantiatePrefabForComponent<IdleClickerScreenView>(_prefabSettings.IdleClickerScreenPrefab);
            screenView.Initialize();
        }

        private void ClearScreens()
        {
            
        }
    }
}
