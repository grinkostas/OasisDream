using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.MarketLogic.Customers
{
    public class CustomersQueue : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Vector3 _queueItemDelta;
        [SerializeField] private float _requestCompleteActualizeDelay;
        [SerializeField] private Vector3 _angles;

        private List<Customer> _customersInQueue = new();

        public int Count => _customersInQueue.Count;

        public void AddCustomer(Customer customer)
        {
            _customersInQueue.Add(customer);
            customer.Move(GetQueuePosition(_customersInQueue.Count - 1), _angles);
            customer.RequestCompleted.Once(OnRequestCompleted);
        }

        private Vector3 GetQueuePosition(int index)
        {
            return _startPoint.position + _queueItemDelta * index;
        }

        private void OnRequestCompleted(Customer customer)
        {
            customer.RequestCompleted.Off(OnRequestCompleted);
            _customersInQueue.Remove(customer);
            DOTween.Kill(this);
            DOVirtual.DelayedCall(_requestCompleteActualizeDelay, Actualize).SetId(this);
        }

        private void Actualize()
        {
            for (int i = 0; i < _customersInQueue.Count; i++)
            {
                _customersInQueue[i].Move(GetQueuePosition(i), _angles);
            }
        }
    }
}