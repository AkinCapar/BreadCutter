using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BreadCutter.Installers
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<LevelSpawnedSignal>().OptionalSubscriber();
            Container.DeclareSignal<BladeSpawnedSignal>().OptionalSubscriber();
            Container.DeclareSignal<BasketSpawnedSignal>().OptionalSubscriber();
            Container.DeclareSignal<AddBreadButtonPressedSignal>().OptionalSubscriber();
            Container.DeclareSignal<BladeSwitchedDirectionSignal>().OptionalSubscriber();
            Container.DeclareSignal<SliceSignal>().OptionalSubscriber();
            Container.DeclareSignal<BreadSlicingDoneSignal>().OptionalSubscriber();
            Container.DeclareSignal<BasketIsLoadedSignal>().OptionalSubscriber();
            Container.DeclareSignal<BasketCanChangeSignal>().OptionalSubscriber();
            Container.DeclareSignal<MergeButtonPressedSignal>().OptionalSubscriber();
            Container.DeclareSignal<MergeAnimationIsDone>().OptionalSubscriber();
            Container.DeclareSignal<PlayerTappingScreenSignal>().OptionalSubscriber();
        }
    }
}
