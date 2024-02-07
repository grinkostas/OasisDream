using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Scripts.MarketLogic.Customers;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class House : MonoBehaviour
    {
        [SerializeField] private List<Customer> _customers;
        [SerializeField] private float _actualizeDelay;
        [SerializeField] private float _customerRequestDelay;

        [Inject] public Island Island { get; }
        
        private void Start()
        {
            foreach (var customer in _customers)
            {
                ApplyRequest(customer);
            }
        }

        private void ApplyRequest(Customer customer)
        {
            var requester = Island.GetRequester();
            if (requester == null)
            {
                customer.MoveToTheStart();
                DOVirtual.DelayedCall(_actualizeDelay, () => ApplyRequest(customer));
                return;
            }
            DOTween.Kill(_actualizeDelay);
            
            var request = requester.GetRequest();
            
            customer.ApplyRequest(request);
            requester.Queue.AddCustomer(customer);

            customer.RequestCompleted.Off(OnRequestCompleted);
            customer.RequestCompleted.Once(OnRequestCompleted);
        }

        private void OnRequestCompleted(Customer customer)
        {
            customer.MoveToTheStart();
            customer.AgentHandler.OnReceiveDestination.Once(_ =>
            {
                DOTween.Kill(customer);
                DOVirtual.DelayedCall(_customerRequestDelay,
                        () => ApplyRequest(customer))
                    .SetId(customer);
            });
            customer.RequestCompleted.Off(OnRequestCompleted);
            
           
        }
    }
}