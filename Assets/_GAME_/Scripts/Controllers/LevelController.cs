using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using BreadCutter.Views;
using UnityEngine;
using Zenject;

namespace BreadCutter.Controllers
{
    public class LevelController : BaseController
    {
        private LevelView _levelView;
        
        #region Injection

        private DiContainer _diContainer;
        private PrefabSettings _prefabSettings;
        private BladeSettings _bladeSettings;
        
        public LevelController(DiContainer diContainer
            , PrefabSettings prefabSettings
            , BladeSettings bladeSettings)
        {
            _diContainer = diContainer;
            _prefabSettings = prefabSettings;
            _bladeSettings = bladeSettings;
        }

        #endregion
        
        public override void Initialize()
        {
            SpawnLevel();
            SpawnBlade();
            _signalBus.Fire(new LevelSpawnedSignal(_levelView));
        }

        private void SpawnLevel()
        {
            _levelView = _diContainer.InstantiatePrefabForComponent<LevelView>(_prefabSettings.LevelPrefab);
        }

        private void SpawnBlade()
        {
            BladeView blade = _diContainer.InstantiatePrefabForComponent<BladeView>(_prefabSettings.BladePrefab);
            blade.transform.position = _levelView.bladePos.position;
            blade.SetData(_bladeSettings.BladeData[0]);
            _signalBus.Fire(new BladeSpawnedSignal(blade));
        }

        public override void Dispose()
        {

        }
    }
}
