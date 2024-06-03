using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadCutter.Models
{
    public class BladeModel
    {
        private int _bladeLevel;
        public int BladeLevel => _bladeLevel;

        private float _bladePower;
        public float BladePower => _bladePower;


        public void IncreaseBladeLevel()
        {
            _bladeLevel++;
        }

        public void SetBladePower(float newPower)
        {
            _bladePower = newPower;
        }
    }
}
