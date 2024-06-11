using System.Collections;
using System.Collections.Generic;
using BreadCutter.Data;
using UnityEngine;

namespace BreadCutter.Settings
{
    [CreateAssetMenu(fileName = nameof(BreadSettings), menuName = Constants.MenuNames.SETTINGS + nameof(BreadSettings))]
    public class BreadSettings : ScriptableObject
    {
        public BreadData[] Breads;
    }
}
