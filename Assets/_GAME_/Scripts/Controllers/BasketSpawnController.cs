using System.Collections;
using System.Collections.Generic;
using BreadCutter.Views;
using UnityEngine;

public class BasketSpawnController : BaseController
{
    private Vector3 _spawnPos;
    private bool spawnWithAdditionalBaskets;
    
    #region Injection

    private BasketView.Factory _basketFactory;
        
    public BasketSpawnController(BasketView.Factory basketFactory)
    {
        _basketFactory = basketFactory;
    }

    #endregion
    
    public override void Initialize()
    {
        _signalBus.Subscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
        _signalBus.Subscribe<BasketCanChangeSignal>(OnBasketCanChangeSignal);
        _signalBus.Subscribe<ConveyorReachedLevelThreeSignal>(OnConveyorReachedLevelThree);
    }
    
    private void OnLevelSpawnedSignal(LevelSpawnedSignal signal)
    {
        _spawnPos = signal.LevelView.basketPos.position;
        SpawnBasket();
    }
    
    private void OnConveyorReachedLevelThree()
    {
        spawnWithAdditionalBaskets = true;
    }

    private void SpawnBasket()
    {
        BasketView basketView = _basketFactory.Create();
        basketView.transform.position = _spawnPos;
        if (spawnWithAdditionalBaskets)
        {
            basketView.RevealAdditionalBaskets();
        }
        _signalBus.Fire(new BasketSpawnedSignal(basketView));
    }

    private void OnBasketCanChangeSignal()
    {
        SpawnBasket();
    }

    public override void Dispose()
    {
        _signalBus.Unsubscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
        _signalBus.Unsubscribe<BasketCanChangeSignal>(OnBasketCanChangeSignal);
        _signalBus.Unsubscribe<ConveyorReachedLevelThreeSignal>(OnConveyorReachedLevelThree);
    }
}
