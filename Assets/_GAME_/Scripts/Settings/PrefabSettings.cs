using System.Collections;
using System.Collections.Generic;
using BreadCutter.Views;
using UnityEngine;

namespace BreadCutter.Settings
{
    [CreateAssetMenu(fileName = nameof(PrefabSettings), menuName = Constants.MenuNames.SETTINGS + nameof(PrefabSettings))]

    public class PrefabSettings : ScriptableObject
    {
        public IdleClickerScreenView IdleClickerScreenPrefab;
        public LevelView LevelPrefab;
        public BladeView BladePrefab;
        public BasketView BasketPrefab;
        public BreadView BreadPrefab;
    }
}
