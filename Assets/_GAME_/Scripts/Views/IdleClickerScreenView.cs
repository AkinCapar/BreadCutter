using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class IdleClickerScreenView : MonoBehaviour
{
    [SerializeField] private Image _cooldownSlider;
    
    #region Injection

    private SignalBus _signalBus;
    private LevelSettings _levelSettings;

    [Inject]
    private void Construct(SignalBus signalBus
        , LevelSettings levelSettings)
    {
        _signalBus = signalBus;
        _levelSettings = levelSettings;
    }

    #endregion

    public void Initialize()
    {
        _signalBus.Subscribe<PlayerCooldownSignal>(OnPlayerCooldownSignal);
    }
    
    public void OnAddButtonPressed()
    {
        _signalBus.Fire<AddBreadButtonPressedSignal>();
    }

    public void OnMergeButtonPressed()
    {
        _signalBus.Fire<MergeButtonPressedSignal>();
    }

    public void OnPlayerCooldownSignal(PlayerCooldownSignal signal)
    {
        float remapValue = 0 + (signal.TimeHeldDown - 0) * (1 - 0) / (_levelSettings.MaxCooldownAmount - 0);

        _cooldownSlider.fillAmount = remapValue;
    }

    public void UpgradeBladeButtonPressed()
    {
        _signalBus.Fire<UpgradeBladeButtonPressedSignal>();
    }

    public void ResetGame()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
