using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class IdleClickerScreenView : MonoBehaviour
{
    #region Injection

    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    #endregion
    
    public void OnAddButtonPressed()
    {
        _signalBus.Fire<AddBreadButtonPressedSignal>();
    }

    public void OnMergeButtonPressed()
    {
        _signalBus.Fire<MergeButtonPressedSignal>();
    }

    public void ResetGame()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
