using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals.Api;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MarketLogic.Customers
{
    [RequireComponent(typeof(BoxCollider))]
    public class MarketRequestExecutor : MonoBehaviour
    {
        [SerializeField] private RequestView _requestView;
        [SerializeField] private float _executeTime;
        [SerializeField] private float _executeNextDelay;
        
        [Inject, UsedImplicitly] public Market Market{ get; }

        private bool _executing = false;
        
        private ISignalCallback _marketCallback;
        private List<Customer> _customersInQueue = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Customer customer))
            {
                if(_customersInQueue.Contains(customer) == false)
                    _customersInQueue.Add(customer);
                ExecuteRequest();
            }
        }

        private void ExecuteRequest()
        {
            if(_customersInQueue.Count <= 0 || _executing)
                return;
            var customer = _customersInQueue[0];
            _executing = true;
            var request = customer.Request;
            _requestView.ApplyRequest(request);
            if (Market.StoredAmount < request.Amount)
            {
                _marketCallback = Market.Added.On(()=>Actualize(customer));
                _requestView.MakeUnavailable();
                return;
            }
        
            CompleteRequest(customer);
        }

        private void Actualize(Customer customer)
        {
            if (Market.StoredAmount < customer.Request.Amount)
                return;
            
            _requestView.MakeAvailable();
            _marketCallback?.Off();
            CompleteRequest(customer);
        }

        private void CompleteRequest(Customer customer)
        {
            DOVirtual.Float(0, 1, _executeTime, value => _requestView.Progress = value)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Market.Sell(customer.Request.Amount);
                    _requestView.CompleteRequest();
                    customer.CompleteRequest();
                    _customersInQueue.Remove(customer);
                    _executing = false;
                    if(_customersInQueue.Count > 0)
                        DOVirtual.DelayedCall(_executeNextDelay, ExecuteRequest);
                });
        }
    }
}