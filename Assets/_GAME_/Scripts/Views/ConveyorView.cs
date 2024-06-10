using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadCutter.Views
{
    public class ConveyorView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _bodyGameobjects;
        public Transform[] breadPositions;
        private int _conveyorLevel = 1;
        public int ConveyorLevel => _conveyorLevel;


        public void Expand()
        {
            _conveyorLevel++;
            Vector3 newScale = new Vector3(_bodyGameobjects[0].transform.localScale.x,
                _bodyGameobjects[0].transform.localScale.y,
                _bodyGameobjects[0].transform.localScale.z + 1);
                
            foreach (GameObject obj in _bodyGameobjects)
            {
                obj.transform.localScale = newScale;
            }
        }
    }
}
