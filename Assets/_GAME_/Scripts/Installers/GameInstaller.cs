using BreadCutter.Controllers;
using BreadCutter.Data;
using BreadCutter.Models;
using BreadCutter.Settings;
using BreadCutter.Views;
using UnityEngine;
using Zenject;

namespace BreadCutter.Installers
{
    public class GameInstaller : MonoInstaller
    {
        #region Injection

        private PrefabSettings _prefabSettings;

        [Inject]
        private void Construct(PrefabSettings prefabSettings)
        {
            _prefabSettings = prefabSettings;
        }

        #endregion

        public override void InstallBindings()
        {
            GameSignalsInstaller.Install(Container);

            //MODELS
            Container.Bind<LevelModel>().AsSingle();
            Container.Bind<BreadModel>().AsSingle();
            Container.Bind<CurrencyModel>().AsSingle();
            Container.Bind<UpgradesModel>().AsSingle();

            //CONTROLLERS
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.Bind<LevelController>().AsSingle();
            Container.Bind<BreadSpawnController>().AsSingle();
            Container.Bind<BasketSpawnController>().AsSingle();
            Container.Bind<BreadController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BladeController>().AsSingle();
            Container.Bind<BasketController>().AsSingle();
            Container.Bind<ScreenController>().AsSingle();
            Container.Bind<SliceController>().AsSingle();
            Container.Bind<MergeController>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle();
            Container.Bind<ConveyorController>().AsSingle();
            Container.Bind<CurrencyController>().AsSingle();
            Container.Bind<UpgradeController>().AsSingle();

            //FACTORIES
            InstallBreads();
            InstallBaskets();
            InstallCoinGainedFXs();
        }

        private void InstallBreads()
        {
            Container.BindFactory<BreadData, int, Vector3, BreadView, BreadView.Factory>()
                .FromPoolableMemoryPool<BreadData, int, Vector3, BreadView, BreadView.Pool>(poolBinder => poolBinder
                    .WithInitialSize(15)
                    .FromComponentInNewPrefab(_prefabSettings.BreadPrefab));
        }

        private void InstallBaskets()
        {
            Container.BindFactory<BasketView, BasketView.Factory>()
                .FromPoolableMemoryPool<BasketView, BasketView.Pool>(poolBinder => poolBinder
                    .WithInitialSize(3)
                    .FromComponentInNewPrefab(_prefabSettings.BasketPrefab));
        }

        private void InstallCoinGainedFXs()
        {
            Container.BindFactory<CoinGainedFXView, CoinGainedFXView.Factory>()
                .FromPoolableMemoryPool<CoinGainedFXView, CoinGainedFXView.Pool>(poolBinder => poolBinder
                    .WithInitialSize(10)
                    .FromComponentInNewPrefab(_prefabSettings.CoinGainedFXPrefab));
        }
    }
}