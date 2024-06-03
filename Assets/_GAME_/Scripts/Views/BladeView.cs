using System;
using BreadCutter.Controllers;
using BreadCutter.Data;
using UnityEngine;
using Zenject;

namespace BreadCutter.Views
{
    public class BladeView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;

        private int _bladeLevel;
        private int movingDirection = -1;
        private bool _waitForTheBasket;
        private bool _shouldStop;
        private float _slicingPower;

        #region Injection

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
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
                transform.position += moveAmount * movingDirection * _slicingPower * Time.deltaTime;
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
                /*int levelDifference = bread.BreadLevel - _bladeLevel;
                _slicingPower += levelDifference **/ 
            }
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
                _signalBus.Fire(new SliceSignal(bread, transform.position, transform.right));
            }
        }
    }
}