using System;
using UnityEngine;

namespace BreadCutter.Data
{
    [Serializable]
    public class BreadData
    {
        public int BreadLevel;
        public Mesh Mesh;
        public float SliceThickness;
        public Vector3 ColliderSize;
        public Vector3 ColliderCenter;
        public float DistanceNeededToSpawnNextBread;
        public Vector3 MeshGOPosition;
        public int MaterialAmount;
    }
}
