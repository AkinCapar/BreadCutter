using System.Collections;
using System.Collections.Generic;
using BreadCutter.Data;
using BreadCutter.Models;
using BreadCutter.Settings;
using BreadCutter.Views;
using UnityEngine;

public class BreadSpawnController : BaseController
{
    private LevelView _currentLevelView;

    #region Injection

    private BreadView.Factory _breadFactory;
    private BreadModel _breadModel;
    private BreadSettings _breadSettings;

    public BreadSpawnController(BreadView.Factory breadFactory
        , BreadModel breadModel
        , BreadSettings breadSettings)
    {
        _breadFactory = breadFactory;
        _breadModel = breadModel;
        _breadSettings = breadSettings;
    }

    #endregion

    public override void Initialize()
    {
        _signalBus.Subscribe<LevelSpawnedSignal>(OnLevelSpawnedSignal);
        _signalBus.Subscribe<AddBreadButtonPressedSignal>(OnAddBreadButtonPressedSignal);
        _signalBus.Subscribe<BreadSlicingDoneSignal>(OnBreadSlicingDoneSignal);
        _signalBus.Subscribe<MergeAnimationIsDone>(OnMergeAnimationIsDoneSignal);
    }

    private void OnLevelSpawnedSignal(LevelSpawnedSignal signal)
    {
        //TODO check it
        _currentLevelView = signal.LevelView;

        /*for (int i = 0; i < _breadModel.ActiveBreadLevels.Count; i++)
        {
            BreadData data = _breadSettings.Breads[0];
            _breadFactory.Create(data, _currentLevelView.breadPositions[i].transform.position);
        }*/
    }

    private void OnAddBreadButtonPressedSignal()
    {
        if (!_breadModel.IsBreadSlotsAreFull)
        {
            BreadData data = _breadSettings.Breads[0];
            List<BreadView> list = new List<BreadView>();
            int lineIndex = _breadModel.GetNextBreadLineIndexToSpawn(_currentLevelView.breadPositions.Length);

            for (int i = 0; i < 10; i++) //TODO fix magic numbers
            {
                Vector3 pos = _currentLevelView.breadPositions[lineIndex].transform.position +
                              (-i * Vector3.right * data.DistanceNeededToSpawnNextBread);

                BreadView bread = _breadFactory.Create(data, lineIndex, pos);
                list.Add(bread);
            }

            _breadModel.BreadLineSpawned(list, _currentLevelView.breadPositions.Length, lineIndex);
        }
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

    private void OnMergeAnimationIsDoneSignal(MergeAnimationIsDone signal)
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
        _breadModel.EmptyBreadSlotCheck(_currentLevelView.breadPositions.Length);
    }

    private List<BreadView> SpawnMergedBreads(List<BreadView> breadList)
    {
        BreadData data = _breadSettings.Breads.Length <= breadList[0].BreadLevel + 1
            ? _breadSettings.Breads[0]
            : _breadSettings.Breads[breadList[0].BreadLevel + 1];


        List<BreadView> list = new List<BreadView>();
        int lineIndex = breadList[0].LineIndex;

        for (int i = 0; i < 10; i++) //TODO fix magic numbers
        {
            Vector3 pos = _currentLevelView.breadPositions[lineIndex].transform.position +
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
        _signalBus.Unsubscribe<MergeAnimationIsDone>(OnMergeAnimationIsDoneSignal);
    }
}