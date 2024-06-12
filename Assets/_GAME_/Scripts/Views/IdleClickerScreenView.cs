using System.Collections;
using System.Collections.Generic;
using BreadCutter.Controllers;
using BreadCutter.Models;
using BreadCutter.Settings;
using BreadCutter.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class IdleClickerScreenView : MonoBehaviour
{
    [SerializeField] private Image _cooldownSlider;
    [SerializeField] private TextMeshProUGUI AddBreadCostText;
    [SerializeField] private TextMeshProUGUI MergeCostText;
    [SerializeField] private TextMeshProUGUI ExpandCostText;
    [SerializeField] private TextMeshProUGUI UpgradeBladeCostText;
    [SerializeField] private TextMeshProUGUI TotalCoinText;

    #region Injection

    private SignalBus _signalBus;
    private LevelSettings _levelSettings;
    private UpgradeController _upgradeController;
    private UpgradesModel _upgradesModel;

    [Inject]
    private void Construct(SignalBus signalBus
        , LevelSettings levelSettings
        , UpgradeController upgradeController
        , UpgradesModel upgradesModel)
    {
        _signalBus = signalBus;
        _levelSettings = levelSettings;
        _upgradeController = upgradeController;
        _upgradesModel = upgradesModel;
    }

    #endregion

    public void Initialize()
    {
        AddBreadCostText.text = "Free";
        MergeCostText.text = "Free";
        ExpandCostText.text = "Free";
        UpgradeBladeCostText.text = "Free";
        TotalCoinText.text = "0";

        _signalBus.Subscribe<PlayerCooldownSignal>(OnPlayerCooldownSignal);
        _signalBus.Subscribe<TotalCoinChangedSignal>(OnTotalCoinChangedSignal);
        _signalBus.Subscribe<MergeOperationStartedSignal>(OnMergeOperationStartedSignal);
        _signalBus.Subscribe<BreadLineSpawnedSignal>(OnBreadLineSpawnedSignal);
        _signalBus.Subscribe<ConveyorExpandedSignal>(OnConveyorExpandedSignal);
        _signalBus.Subscribe<BladeUpgradedSignal>(OnBladeUpgradedSignal);
    }

    public void OnAddButtonPressed()
    {
        _signalBus.Fire<AddBreadButtonPressedSignal>();
    }

    private void OnBreadLineSpawnedSignal()
    {
        AddBreadCostText.text = _upgradesModel.AddBreadValue.ToString();
    }

    public void OnMergeButtonPressed()
    {
        _signalBus.Fire<MergeButtonPressedSignal>();
    }

    private void OnMergeOperationStartedSignal()
    {
        MergeCostText.text = _upgradesModel.MergeValue.ToString();
    }

    public void OnPlayerCooldownSignal(PlayerCooldownSignal signal)
    {
        float remapValue = 0 + (signal.TimeHeldDown - 0) * (1 - 0) / (_levelSettings.MaxCooldownAmount - 0);

        _cooldownSlider.fillAmount = remapValue;
    }

    public void OnExpandButtonPressed()
    {
        _signalBus.Fire<ExpandButtonPressedSignal>();
    }

    private void OnConveyorExpandedSignal()
    {
        ExpandCostText.text = _upgradesModel.ExpandValue.ToString();
    }

    public void UpgradeBladeButtonPressed()
    {
        _signalBus.Fire<UpgradeBladeButtonPressedSignal>();
    }

    private void OnBladeUpgradedSignal()
    {
        UpgradeBladeCostText.text = _upgradesModel.UpgradeBladeValue.ToString();
    }

    private void OnTotalCoinChangedSignal(TotalCoinChangedSignal signal)
    {
        TotalCoinText.text = signal.Value.ToString();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}