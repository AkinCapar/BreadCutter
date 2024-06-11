using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadCutter.Utils
{
    public enum GameStates
    {
        WaitingToStartState,
        Playing,
    }

    public enum ScreenStates
    {
        IdleClickerState
    }

    public enum CurrencyType
    {
        Coin
    }
    public enum UpgradeTypes
    {
        AddBread,
        BladeUpgrade,
        Merge,
        Expand
    }
}
