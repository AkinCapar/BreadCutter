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

            //FACTORIES
            InstallBreads();
            InstallBaskets();
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
                    .WithInitialSize(5)
                    .FromComponentInNewPrefab(_prefabSettings.BasketPrefab));
        }
    }
}