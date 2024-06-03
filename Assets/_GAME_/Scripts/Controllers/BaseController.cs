using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class BaseController
{
    #region Injection

    protected SignalBus _signalBus;
        
    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    #endregion

    public abstract void Initialize();
    public abstract void Dispose();
}