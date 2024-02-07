using DG.Tweening;
using NepixSignals;
using StaserSDK.Stack;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCore.Scripts.MarketLogic.Customers
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private NavMeshAgentHandler _agent;
        [SerializeField] private Transform _startPoint;

        public NavMeshAgentHandler AgentHandler => _agent;
        public Request Request { get; private set; }

        public TheSignal<Request> RequestApplied { get; } = new();
        public TheSignal<Customer> RequestCompleted { get; } = new();
        
        public void ApplyRequest(Request request)
        {
            Request = request;
            RequestApplied.Dispatch(request);
        }

        public void CompleteRequest()
        {
            RequestCompleted.Dispatch(this);
        }

        public void MoveToTheStart()
        {
            Move(_startPoint.position, _startPoint.rotation.eulerAngles);
        }
        
        public void Move(Vector3 destination, Vector3 angles)
        {
            _agent.SetDestination(destination);
            _agent.OnReceiveDestination.Once(handler => OnReceiveDestination(handler, angles));
        }

        private void OnReceiveDestination(NavMeshAgentHandler agentHandler, Vector3 angles)
        {
            DOTween.Kill(_agent);
            _agent.transform.DORotate(angles, 0.35f).SetId(_agent);
        }

    }
}