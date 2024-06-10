using System;
using System.Data.Common;
using BreadCutter.Controllers;
using BreadCutter.Data;
using BreadCutter.Settings;
using UnityEngine;
using Zenject;

namespace BreadCutter.Views
{
    public class BladeView : MonoBehaviour
    {
        [SerializeField] private GameObject _bladeBody;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private float _defaultSlicingPower;

        private int _bladeLevel;
        public int BladeLevel => _bladeLevel;
        private int movingDirection = -1;
        private bool _waitForTheBasket;
        private bool _shouldStop;
        private float _slicingPower;
        private int _currentlyCutting;
        private float _inputPower;

        #region Injection

        private SignalBus _signalBus;
        private LevelSettings _levelSettings;

        [Inject]
        private void Construct(SignalBus signalBus
            , LevelSettings levelSettings)
        {
            _signalBus = signalBus;
            _levelSettings = levelSettings;
        }

        #endregion

        public void SetData(BladeData data)
        {
            _slicingPower = 1;
            _meshFilter.mesh = data.Mesh;
            _bladeLevel = data.BladeLevel;
        }

        public void MoveBlade(Vector3 moveAmount)
        {
            if (!_shouldStop)
            {
                transform.position += moveAmount * movingDirection * _slicingPower * _inputPower * Time.deltaTime;
            }
        }

        public void TurnBlade(float turnSpeed)
        {
            if (!_shouldStop)
            {
                _bladeBody.transform.Rotate(Vector3.up, turnSpeed * _inputPower * Time.deltaTime);
            }
        }

        public void UpgradeBlade(BladeData data)
        {
            _meshFilter.mesh = data.Mesh;
            _bladeLevel = data.BladeLevel;
        }

        public void SlicingBreadDespawned()
        {
            _currentlyCutting--;
            if (_currentlyCutting == 0)
            {
                _slicingPower = _defaultSlicingPower;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.BladeBorder))
            {
                _signalBus.Fire<BladeSwitchedDirectionSignal>();
                movingDirection *= -1;
                if (_waitForTheBasket)
                {
                    _shouldStop = true;
                    _signalBus.Fire<BasketCanChangeSignal>();
                }
            }

            if (other.TryGetComponent(out BreadView bread))
            {
                int levelDifference = bread.BreadLevel - _bladeLevel;
                float newSlicingPower =
                    (_defaultSlicingPower - levelDifference * _levelSettings.BladeBreadLevelDifferenceEffectAmount) - _levelSettings.DefaultBladeSlicingDelay;

                if (newSlicingPower <= 0)
                {
                    _slicingPower = 0.1f;
                }
                else
                {
                    _slicingPower = newSlicingPower;
                }

                _currentlyCutting++;
            }
        }

        public void SetInputPower(float inputPower)
        {
            _inputPower = inputPower;
        }

        public void WaitForBasketChange()
        {
            _waitForTheBasket = true;
        }

        public void BladeCanMove()
        {
            _waitForTheBasket = false;
            _shouldStop = false;
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out BreadView bread))
            {
                _currentlyCutting--;
                _signalBus.Fire(new SliceSignal(bread, transform.position, transform.right));

                if (_currentlyCutting == 0)
                {
                    _slicingPower = _defaultSlicingPower;
                }
            }
        }
    }
}