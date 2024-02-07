using System;
using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MarketLogic.Customers
{
    public class Requester : MonoBehaviour
    {
        [SerializeField] private CustomersQueue _queue;
        [SerializeField] private ItemType _requestItemType;
        [SerializeField] private Vector2Int _requestAmountRange;

        [Inject] public Island Island { get; }

        private List<Request> _queuedRequests = new();
        public List<Request> QueuedRequests => _queuedRequests;

        public ItemType RequestedType => _requestItemType;

        public CustomersQueue Queue => _queue;

        public Request GetRequest()
        {
            if (_queuedRequests.Count > 0)
            {
                var request = _queuedRequests[0];
                _queuedRequests.Remove(request);
                return request;
            }
            return new Request(_requestItemType, _requestAmountRange.Random());
        }

        private void OnEnable()
        {
            Island.AddRequester(this);
        }

        private void OnDisable()
        {
            Island.RemoveRequester(this);
        }

        public void AddRequest(Request request)
        {
            if(request.Type != _requestItemType)
                return;
            _queuedRequests.Add(request);
        }
    }
}