using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using UnityEngine;
using Zenject;

namespace BreadCutter.Views
{
    public class ConveyorView : MonoBehaviour
    {
        [SerializeField] private GameObject[] _bodyGameobjects;
        public Transform[] breadPositions;
        private int _conveyorLevel = 1;
        public int ConveyorLevel => _conveyorLevel;

        #region Injection

        private LevelSettings _levelSettings;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(LevelSettings levelSettings
            , SignalBus signalBus)
        {
            _levelSettings = levelSettings;
            _signalBus = signalBus;
        }

        #endregion

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

            if (_conveyorLevel == 3)
            {
                _signalBus.Fire<ConveyorReachedLevelThreeSignal>();
            }
        }
    }
}