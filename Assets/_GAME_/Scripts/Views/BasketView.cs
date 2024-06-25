using System;
using System.Collections;
using System.Collections.Generic;
using BreadCutter.Settings;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BreadCutter.Views
{
    public class BasketView : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private List<GameObject> _slices = new List<GameObject>();
        private int _sliceCount;
        private bool _isLoaded;
        private float _startPosZ;
        
        private IMemoryPool _pool;
        
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
        
        public void OnSpawned(IMemoryPool pool)
        {
            _sliceCount = 0;
            _isLoaded = false;
            _pool = pool;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.Slice))
            {
                _sliceCount++;
                other.gameObject.transform.parent = transform;
                _slices.Add(other.gameObject);
                
                if (_sliceCount >= _levelSettings.BasketMaxSliceAmount && !_isLoaded)
                {
                    _isLoaded = true;
                    _signalBus.Fire<BasketIsLoadedSignal>();
                }
            }
        }

        public void MoveLoadedBasket(float moveDuration)
        {
            transform.DOMoveZ(-2 * _startPosZ, moveDuration * 2).OnComplete(() =>
            {
                foreach (GameObject slice in _slices)
                {
                    Destroy(slice);
                }
                _slices.Clear();
                _pool.Despawn(this);
            });
        }

        public void MoveToCollectionSpot(float moveDuration)
        {
            _startPosZ = transform.position.z;
            transform.DOMoveZ(0, moveDuration);
        }
        
        public void OnDespawned()
        {
        }
        
        public class Factory : PlaceholderFactory<BasketView>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, BasketView>
        {
        }
    }
}
