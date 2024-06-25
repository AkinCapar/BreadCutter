using System.Collections;
using System.Collections.Generic;
using BreadCutter.Controllers;
using BreadCutter.Views;
using UnityEngine;

public class BasketSpawnController : BaseController
{
    private Vector3 _spawnPos;
    
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
    }
    
    private void OnLevelSpawnedSignal(LevelSpawnedSignal signal)
    {
        _spawnPos = signal.LevelView.basketPos.position;
        SpawnInitialBaskets();
    }

    private void SpawnInitialBaskets()
    {
        List<BasketView> baskets = new List<BasketView>();
        for (int i = 0; i < 3; i++)
        {
            BasketView basketView = _basketFactory.Create();
            basketView.transform.position = _spawnPos + Vector3.forward * 6 * i;
            baskets.Add(basketView);
        }
        
        _signalBus.Fire(new InitialBasketsSpawned(baskets));
    }

    private void SpawnBasket()
    {
        BasketView basketView = _basketFactory.Create();
        basketView.transform.position = _spawnPos;
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
    }
}
