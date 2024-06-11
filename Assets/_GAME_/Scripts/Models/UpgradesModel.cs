using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadCutter.Models
{
    public class UpgradesModel
    {
        public int AddBreadValue { get; private set; }
        public int AddBreadLevel { get; private set; }
        public int MergeValue { get; private set; }
        public int MergeLevel { get; private set; }
        public int ExpandValue { get; private set; }
        public int ExpandLevel { get; private set; }
        public int UpgradeBladeValue { get; private set; }
        public int UpgradeBladeLevel { get; private set; }

        public void UpdateAddBreadValue(int value)
        {
            AddBreadValue = value;
            AddBreadLevel++;
        }

        public void UpdateMergeValue(int value)
        {
            MergeValue = value;
            MergeLevel++;
        }

        public void UpdateExpandValue(int value)
        {
            ExpandValue = value;
            ExpandLevel++;
        }

        public void UpdateUpgradeBladeValue(int value)
        {
            UpgradeBladeValue = value;
            UpgradeBladeLevel++;
        }
    }
}
