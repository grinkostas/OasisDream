using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Utilities;
using Zenject;

public class DestructibleStateEnabler : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private List<EnableData> _enableData;

    [Inject] private Timer _timer;
    
    private void OnEnable()
    {
        _destructible.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        if(_destructible.Health <= 0)
            HandleChange(TriggerAction.OnDie);
        if (_destructible.Health == _destructible.MaxHealth)
            HandleChange(TriggerAction.OnRespawn);
    }

    
    private void HandleChange(TriggerAction triggerAction)
    {
        _enableData.FindAll(x => x.TriggerAction == triggerAction).ForEach(HandleAction);
    }

    private void HandleAction(EnableData enableData)
    {
        bool active = enableData.Action == DestructibleAction.Show;
        _timer.ExecuteWithDelay(() => enableData.GameObject.SetActive(active), enableData.ActionDelay);
    }


    [System.Serializable]
    private class EnableData
    {
        public GameObject GameObject;
        public TriggerAction TriggerAction;
        public DestructibleAction Action;
        public float ActionDelay = 0.0f;
    }
    
    private enum TriggerAction
    {
        OnRespawn,
        OnDie
    }

    private enum DestructibleAction
    {
        Show, 
        Hide
    }
}
