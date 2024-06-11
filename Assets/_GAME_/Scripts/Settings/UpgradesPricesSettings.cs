using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadCutter.Settings
{
    [CreateAssetMenu(fileName = nameof(UpgradesPricesSettings), menuName = Constants.MenuNames.SETTINGS + nameof(UpgradesPricesSettings))]
    public class UpgradesPricesSettings : ScriptableObject
    {
        public int[] AddBreadPrices;
        public int[] MergePrices;
        public int[] ExpandPrices;
        public int[] UpgradeBladePrices;
    }
}
