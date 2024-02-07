using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Stack;

public class RecyclerFx : MonoBehaviour
{
    [SerializeField] private Recycler _recycler;
    [SerializeField] private StackProvider _sourceStack;
    [SerializeField] private StackProvider _productionStack;
    [SerializeField] private StackItemLocator _sourceStackLocator;
    [SerializeField] private StackItemLocator _productionStackLocator;
    
    [SerializeField] private float _zoomTime;
    
    [SerializeField] private Bouncer _bouncer;
    [SerializeField] private float _bounceDelay;
    [Space] 
    [SerializeField] private ParticleSystem _sleepParticle;

    private Coroutine _bounceRoutine;
    private bool _started = false;
    private void OnEnable()
    {
        _sourceStack.Interface.TookItem += OnSourceItemTook;
        _sourceStack.Interface.AddedItem += OnSourceStackAddItem;
        _productionStack.Interface.AddedItem += OnProductionStackAddItem;
        _recycler.OnStartRecycle += OnStartRecycle;
        _recycler.OnEndRecycle += OnEndRecycle;
    }

    private void OnDisable()
    {
        _sourceStack.Interface.TookItem -= OnSourceItemTook;
        _sourceStack.Interface.AddedItem -= OnSourceStackAddItem;
        _productionStack.Interface.AddedItem -= OnProductionStackAddItem;
        _recycler.OnStartRecycle -= OnStartRecycle;
        _recycler.OnEndRecycle -= OnEndRecycle;
    }

    
    private void OnSourceItemTook(StackItemData takeData)
    {
        takeData.Target.Claim();
        takeData.Target.transform.DOScale(Vector3.zero, _zoomTime).OnComplete(
            ()=>takeData.Target.Item.Pool.Return(takeData.Target.Item));
        _sourceStackLocator.Rebuild();
    }
    private void OnSourceStackAddItem(StackItemData addData)
    {
        _sourceStackLocator.Rebuild();
    }
    private void OnProductionStackAddItem(StackItemData addData)
    {
        Transform target = addData.Target.transform;
        target.localScale = Vector3.zero;
        target.localPosition = _productionStackLocator.GetCurrentDelta();
        target.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
        _productionStackLocator.Rebuild(); _productionStackLocator.Rebuild();
    }

    private void OnStartRecycle()
    {
        _sleepParticle.Stop();
        Bounce();
    }

    private void OnEndRecycle()
    {
        _sleepParticle.Play();
        DOTween.Kill(this);
    }

    private void Bounce()
    {
        _bouncer.Bounce();
        DOVirtual.DelayedCall(_bounceDelay, Bounce).SetId(this);
    }
}
