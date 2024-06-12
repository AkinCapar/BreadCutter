using System.Collections;
using System.Collections.Generic;
using BreadCutter.Controllers;
using BreadCutter.Data;
using BreadCutter.Models;
using BreadCutter.Settings;
using BreadCutter.Utils;
using BreadCutter.Views;
using UnityEngine;

public class BreadSpawnController : BaseController
{
    private LevelView _currentLevelView;
    private int _availableLineAmount;

    #region Injection

    private BreadView.Factory _breadFactory;
    private BreadModel _breadModel;
    private BreadSettings _breadSettings;
    private LevelSettings _levelSettings;
    private UpgradeController _upgradeController;

    public BreadSpawnController(BreadView.Factory breadFactory
        , BreadModel breadModel
        , BreadSettings breadSettings
        , LevelSettings levelSettings
        , UpgradeController upgradeController)
    {
        _breadFactory = breadFactory;
        _breadModel = breadModel;
        _breadSettings = breadSettings;
        _levelSettings = levelSettings;
        _upgradeController = upgradeController;
    }

    #endregion

    public override void Initialize()
    {
        _signalBus.Subscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
        _signalBus.Subscribe<AddBreadButtonPressedSignal>(OnAddBreadButtonPressedSignal);
        _signalBus.Subscribe<BreadSlicingDoneSignal>(OnBreadSlicingDoneSignal);
        _signalBus.Subscribe<MergeAnimationIsDoneSignal>(OnMergeAnimationIsDoneSignal);
        _signalBus.Subscribe<ConveyorExpandedSignal>(OnConveyorExpandedSignal);
    }

    private void OnLevelSpawnedSignal(LevelSpawnedSignal signal)
    {
        _currentLevelView = signal.LevelView;
    }

    private void OnAddBreadButtonPressedSignal()
    {
        if (!_breadModel.IsBreadSlotsAreFull && _upgradeController.SpendCoinForUpgrade(UpgradeTypes.AddBread))
        {
            SpawnBreadLine();
            _signalBus.Fire<BreadLineSpawnedSignal>();
        }
    }
    
    private void SpawnBreadLine()
    {
        BreadData data = _breadSettings.Breads[0];
        List<BreadView> list = new List<BreadView>();


        if (_availableLineAmount == 0) { SetAvailableLineAmount();}

        int lineIndex = _breadModel.GetNextBreadLineIndexToSpawn(_availableLineAmount);

        for (int i = 0; i < _levelSettings.LineBreadAmount; i++)
        {
            Vector3 pos = _currentLevelView.conveyorView.breadPositions[lineIndex].transform.position +
                          (-i * Vector3.right * data.DistanceNeededToSpawnNextBread);

            BreadView bread = _breadFactory.Create(data, lineIndex, pos);
            list.Add(bread);
        }

        _breadModel.BreadLineSpawned(list, _currentLevelView.conveyorView.breadPositions.Length, lineIndex);
        _breadModel.EmptyBreadSlotCheck(_availableLineAmount);
    }

    private void OnBreadSlicingDoneSignal(BreadSlicingDoneSignal signal)
    {
        SpawnSingleBread(signal.LineIndex, signal.BreadLevel);
    }

    private void SpawnSingleBread(int lineIndex, int breadLevel)
    {
        BreadData data = _breadSettings.Breads[breadLevel];

        Vector3 pos = _breadModel.GetLastBreadInLine(lineIndex).transform.position -
                      Vector3.right * data.DistanceNeededToSpawnNextBread;

        BreadView bread = _breadFactory.Create(data, lineIndex, pos);
        _breadModel.SingleBreadSpawned(lineIndex, bread);
    }

    private void OnMergeAnimationIsDoneSignal(MergeAnimationIsDoneSignal signal)
    {
        List<BreadView> newList = SpawnMergedBreads(signal.List1);

        foreach (BreadView bread in signal.List1)
        {
            bread.DespawnBread();
        }

        foreach (BreadView bread in signal.List2)
        {
            bread.DespawnBread();
        }

        signal.List1.Clear();
        signal.List2.Clear();

        signal.List1.AddRange(newList);
        _breadModel.EmptyBreadSlotCheck(_availableLineAmount);
    }

    private void OnConveyorExpandedSignal()
    {
        SetAvailableLineAmount();
        _breadModel.EmptyBreadSlotCheck(_availableLineAmount);
    }

    private void SetAvailableLineAmount()
    {
        _availableLineAmount = _currentLevelView.conveyorView.ConveyorLevel == 1
            ? _currentLevelView.conveyorView.ConveyorLevel
            : _currentLevelView.conveyorView.ConveyorLevel * 2 - 1;
    }

    private List<BreadView> SpawnMergedBreads(List<BreadView> breadList)
    {
        BreadData data = _breadSettings.Breads.Length <= breadList[0].BreadLevel + 1
            ? _breadSettings.Breads[0]
            : _breadSettings.Breads[breadList[0].BreadLevel + 1];


        List<BreadView> list = new List<BreadView>();
        int lineIndex = breadList[0].LineIndex;

        for (int i = 0; i < _levelSettings.LineBreadAmount; i++)
        {
            Vector3 pos = _currentLevelView.conveyorView.breadPositions[lineIndex].transform.position +
                          (-i * Vector3.right * data.DistanceNeededToSpawnNextBread);

            BreadView newBread = _breadFactory.Create(data, lineIndex, pos);
            list.Add(newBread);
        }

        return list;
    }

    public override void Dispose()
    {
        _signalBus.Unsubscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
        _signalBus.Unsubscribe<AddBreadButtonPressedSignal>(OnAddBreadButtonPressedSignal);
        _signalBus.Unsubscribe<BreadSlicingDoneSignal>(OnBreadSlicingDoneSignal);
        _signalBus.Unsubscribe<MergeAnimationIsDoneSignal>(OnMergeAnimationIsDoneSignal);
        _signalBus.Unsubscribe<ConveyorExpandedSignal>(OnConveyorExpandedSignal);
    }
}