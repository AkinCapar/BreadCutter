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
    }

    public void OnAddButtonPressed()
    {
        if (_upgradeController.SpendCoinForUpgrade(UpgradeTypes.AddBread))
        {
            _signalBus.Fire<AddBreadButtonPressedSignal>();
            AddBreadCostText.text = _upgradesModel.AddBreadValue.ToString();
        }
    }

    public void OnMergeButtonPressed()
    {
        if (_upgradeController.SpendCoinForUpgrade(UpgradeTypes.Merge))
        {
            _signalBus.Fire<MergeButtonPressedSignal>();
            MergeCostText.text = _upgradesModel.MergeValue.ToString();
        }
    }

    public void OnPlayerCooldownSignal(PlayerCooldownSignal signal)
    {
        float remapValue = 0 + (signal.TimeHeldDown - 0) * (1 - 0) / (_levelSettings.MaxCooldownAmount - 0);

        _cooldownSlider.fillAmount = remapValue;
    }

    public void OnExpandButtonPressed()
    {
        if (_upgradeController.SpendCoinForUpgrade(UpgradeTypes.Expand))
        {
            _signalBus.Fire<ExpandButtonPressedSignal>();
            ExpandCostText.text = _upgradesModel.ExpandValue.ToString();
        }
    }

    public void UpgradeBladeButtonPressed()
    {
        if (_upgradeController.SpendCoinForUpgrade(UpgradeTypes.BladeUpgrade))
        {
            _signalBus.Fire<UpgradeBladeButtonPressedSignal>();
            UpgradeBladeCostText.text = _upgradesModel.UpgradeBladeValue.ToString();
        }
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