using System;
using System.Collections;
using System.Collections.Generic;
using BreadCutter.Data;
using BreadCutter.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace BreadCutter.Views
{
    public class BreadView : MonoBehaviour, IPoolable<BreadData, int, Vector3, IMemoryPool>
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private float _moveDuration;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private GameObject _mainMeshGO;
        [SerializeField] private MeshRenderer _meshRenderer;
        private float _sliceThickness;
        private Vector3 _colliderSize;
        private Vector3 _colliderCenter;
        private List<GameObject> _trashObjects = new List<GameObject>();
        private bool _isBeingSliced;
        private int _lineIndex;
        public int LineIndex => _lineIndex;
        private int _breadLevel;
        public int BreadLevel => _breadLevel;
        public Vector3 _sliceSize;
        public GameObject breadMeshGO;
        public Material sliceMaterial;
        
        private IMemoryPool _pool;
        
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
        
        public void OnSpawned(BreadData data, int lineIndex, Vector3 pos, IMemoryPool pool)
        {
            SetMeshRendererMaterialAmount(data.MaterialAmount);
            _isBeingSliced = false;
            transform.position = pos;
            _breadLevel = data.BreadLevel;
            _meshFilter.mesh = data.Mesh;
            _sliceThickness = data.SliceThickness;
            _colliderSize = data.ColliderSize;
            _colliderCenter = data.ColliderCenter;
            _boxCollider.size = _colliderSize;
            _boxCollider.center = _colliderCenter;
            _boxCollider.enabled = true;
            _mainMeshGO.SetActive(true);
            breadMeshGO = _mainMeshGO;
            _mainMeshGO.transform.localPosition = data.MeshGOPosition;
            _lineIndex = lineIndex;
            _pool = pool;
        }

        private void SetMeshRendererMaterialAmount(int dataMatAmount)
        {
            if (_meshRenderer.materials.Length != dataMatAmount)
            {
                Material[] currentMaterials = _meshRenderer.materials;
                Material[] newMaterials = new Material[dataMatAmount];

                for (int i = 0; i < newMaterials.Length; i++)
                {
                    if (i < currentMaterials.Length)
                    {
                        newMaterials[i] = currentMaterials[i];
                    }

                    else
                    {
                        newMaterials[i] = newMaterials[i - 1];
                    }
                }

                _meshRenderer.materials = newMaterials;
            }
        }

        public void MoveForward()
        {
            transform.DOMoveX(gameObject.transform.position.x + _sliceThickness, _moveDuration);
        }

        public void OnSliced(GameObject newBreadMeshGO)
        {
            Vector3 newColliderSize = _boxCollider.size - Vector3.right * _sliceThickness;
            Vector3 nextColliderSize = _boxCollider.size - Vector3.right * _sliceThickness * 2;
            if (nextColliderSize.x <= 0)
            {
                BreadSlicingDone().Forget();
            }
            
            _boxCollider.size = newColliderSize;
            _boxCollider.center -= Vector3.right * _sliceThickness / 2;
            breadMeshGO.SetActive(false);
            _trashObjects.Add(newBreadMeshGO);
            breadMeshGO = newBreadMeshGO;
        }
        
        private async UniTask BreadSlicingDone()
        {
            _boxCollider.enabled = false;
            _signalBus.Fire(new BreadSlicingDoneSignal(_lineIndex, _breadLevel));
            MoveForward();
            
            await UniTask.WaitForSeconds(_moveDuration);
            
            _trashObjects.Remove(breadMeshGO);
            breadMeshGO.transform.parent = null;
            breadMeshGO.AddComponent<BoxCollider>().size = _sliceSize;;
            breadMeshGO.AddComponent<Rigidbody>();
            breadMeshGO.tag = Constants.Tags.Slice;
            
            _pool.Despawn(this);
        }

        public void Merge(Vector3 mergePos)
        {
            transform.DOJump(mergePos, 3, 1, _levelSettings.MergeAnimationTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.Blade))
            {
                _isBeingSliced = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constants.Tags.Blade))
            {
                _isBeingSliced = false;
            }
        }

        public void DespawnBread()
        {
            if (_isBeingSliced)
            {
                _signalBus.Fire<SlicingBreadDespawnedSignal>();
            }
            _pool.Despawn(this);
        }
        
        public void OnDespawned()
        {
            foreach (GameObject go in _trashObjects)
            {
                Destroy(go);
            }
            _trashObjects.Clear();
        }
        
        public class Factory : PlaceholderFactory<BreadData, int, Vector3, BreadView>
        {
        }

        public class Pool : MonoPoolableMemoryPool<BreadData, int, Vector3, IMemoryPool, BreadView>
        {
        }
    }
}
