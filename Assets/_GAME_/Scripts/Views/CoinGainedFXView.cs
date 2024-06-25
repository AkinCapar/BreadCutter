using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace BreadCutter.Views
{
    public class CoinGainedFXView : MonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField] private float _moveYAmount;
        [SerializeField] private float _moveTime;
        [SerializeField] private TextMeshProUGUI _earnedAmountText;
        private IMemoryPool _pool;
        
        public void OnDespawned()
        {
            transform.SetParent(null);
        }

        public void OnSpawned(IMemoryPool p1)
        {
            _pool = p1;
            transform.DOMoveY(transform.position.y + _moveYAmount, _moveTime);
        }

        private void GoBackToPool()
        {
            _pool.Despawn(this);
        }

        public void SetTheEarnedAmount(int amount)
        {
            _earnedAmountText.text = amount.ToString();
            transform.localScale = Vector3.one;
        }
        
        public class Factory : PlaceholderFactory<CoinGainedFXView>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, CoinGainedFXView>
        {
        }
    }
}
