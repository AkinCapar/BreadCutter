using System.Collections;
using System.Collections.Generic;
using BreadCutter.Data;
using UnityEngine;

namespace BreadCutter.Settings
{
    [CreateAssetMenu(fileName = nameof(BladeSettings), menuName = Constants.MenuNames.SETTINGS + nameof(BladeSettings))]
    public class BladeSettings : ScriptableObject
    {
        public BladeData[] BladeData;
    }
}
