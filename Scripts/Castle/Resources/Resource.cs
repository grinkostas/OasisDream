using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public class Resource : StackItem
{
    [SerializeField] private Collider _collider;
    [SerializeField] private float _timeToEnableCollect;
    [SerializeField] private List<GameObject> _disableOnClaim;
    
    private TimerDelay _spawnTimer;
    
    private void OnEnable()
    {
        _collider.enabled = false;
        if (IsClaimed == false)
            DOVirtual.DelayedCall(_timeToEnableCollect, () =>
            {
                if(IsClaimed == false) 
                    _collider.enabled = true;
            }).SetId(this);
    }
   

    public override void UnClaim()
    {
        Restore();
        _collider.enabled = true;
        UnClaimed?.Invoke(this);
    }

    public override void OnClaim()
    {
        IsClaimed = true;
        _collider.enabled = false;
        _disableOnClaim.ForEach(x=> x.SetActive(false));
    }

    public override void Restore()
    {
        IsClaimed = false;
        _disableOnClaim.ForEach(x=> x.SetActive(true));
    }
}
