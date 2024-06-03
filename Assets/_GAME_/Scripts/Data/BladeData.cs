using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadCutter.Data
{
    [Serializable]
    public class BladeData
    {
        public int BladeLevel;
        public Mesh Mesh;
        public Vector3 ColliderSize;
    }
}
