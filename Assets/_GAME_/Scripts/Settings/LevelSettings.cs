using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BreadCutter.Settings
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = Constants.MenuNames.SETTINGS + nameof(LevelSettings))]

    public class LevelSettings : ScriptableObject
    {
        public float BladeInitialSpeed;
        public float BladeWaitTime;
        public float BasketMaxSliceAmount;
        public float BasketMoveDuration;
        public float BasketWaitTime;
        public float MergeAnimationTime;
        public float BladeBreadLevelDifferenceEffectAmount;
        public float DefaultBladeSlicingDelay;
        public float MaxCooldownAmount;
        public float PlayerInputEffectAmount;
        public float BladeTurnSpeed;
        public int LineBreadAmount;
        public int MaxConveyorLevel;
    }
}
