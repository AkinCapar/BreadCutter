using System.Collections;
using System.Collections.Generic;
using BreadCutter.Views;
using JetBrains.Annotations;
using UnityEngine;

public readonly struct LevelSpawnedSignal
{
    public readonly LevelView LevelView;

    public LevelSpawnedSignal(LevelView levelView)
    {
        LevelView = levelView;
    }
}

public readonly struct BladeSpawnedSignal
{
    public readonly BladeView Blade;

    public BladeSpawnedSignal(BladeView blade)
    {
        Blade = blade;
    }
}

public readonly struct BasketSpawnedSignal
{
    public readonly BasketView Basket;

    public BasketSpawnedSignal(BasketView basket)
    {
        Basket = basket;
    }
}

public readonly struct AddBreadButtonPressedSignal
{
}

public readonly struct BladeSwitchedDirectionSignal
{
}

public readonly struct SliceSignal
{
    public readonly BreadView SliceObject;
    public readonly Vector3 SlicePosition;
    public readonly Vector3 SliceDirection;

    public SliceSignal(BreadView sliceObject, Vector3 slicePosition, Vector3 sliceDirection)
    {
        SliceObject = sliceObject;
        SlicePosition = slicePosition;
        SliceDirection = sliceDirection;
    }
}

public readonly struct BreadSlicingDoneSignal
{
    public readonly int LineIndex;
    public readonly int BreadLevel;

    public BreadSlicingDoneSignal(int lineIndex, int breadLevel)
    {
        LineIndex = lineIndex;
        BreadLevel = breadLevel;
    }
}

public readonly struct BasketIsLoadedSignal
{
    
}

public readonly struct BasketCanChangeSignal
{
    
}

public readonly struct MergeButtonPressedSignal
{
    
}

public readonly struct MergeAnimationIsDone
{
    public readonly List<BreadView> List1;
    public readonly List<BreadView> List2;

    public MergeAnimationIsDone(List<BreadView> list1, List<BreadView> list2)
    {
        List1 = list1;
        List2 = list2;
    }
}

public readonly struct SlicingBreadDespawned
{
    
}

public readonly struct PlayerCooldownSignal
{
    public readonly float TimeHeldDown;

    public PlayerCooldownSignal(float timeHeldDown)
    {
        TimeHeldDown = timeHeldDown;
    }
}

public readonly struct UpgradeBladeButtonPressedSignal
{
    
}