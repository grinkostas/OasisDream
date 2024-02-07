using System;
using GameCore.Scripts.MarketLogic;
using GameCore.Scripts.MarketLogic.Customers;
using UnityEngine;

namespace GameCore.Scripts.Castle.Tutorial.Tasks
{
    public class TutorialStepMarketRequester : MonoBehaviour
    {
        [SerializeField] private Requester _requester;
        [SerializeField] private int _requestAmount;

        private Request _request;
        
        private void OnEnable()
        {
            _request = new Request(_requester.RequestedType, _requestAmount);
            _requester.AddRequest(_request);
        }

        private void OnDisable()
        {
            _requester.QueuedRequests.Remove(_request);
        }

    }
}