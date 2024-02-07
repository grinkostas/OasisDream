using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;

public class BeamFactoryFx : RecycleAdditionalFx
{
    [Header("Buggy")]
    [SerializeField] private Transform _buggy;
    [SerializeField] private Transform _resourcePoint;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _returnTime;
    [SerializeField] private float _stayTime;
    [Header("Resource")]
    [SerializeField] private float _zoomTime;
    [SerializeField] private float _resourceSwapDelay;
    [Header("Factory")] 
    [SerializeField] private float _enableFxDelay;
    [SerializeField] private float _disableFxDelay;
    [SerializeField] private List<Rotator> _rotators;
    [SerializeField] private List<ParticleSystem> _particleSystems;

    private void OnValidate()
    {
        _moveTime = (CycleDuration - _returnTime - _stayTime * 2);
    }

    protected override void OnEnableInternal()
    {
        InitItem(SourceItem);
        InitItem(ProductionItem);
        SetRotatorActive(false);
        SetParticlesActive(false);
    }

    private void InitItem(Transform item)
    {
        item.SetParent(_resourcePoint);
        item.transform.localPosition = Vector3.zero;
    }
    
    protected override void ProductCycle()
    {
        StartCycle();
        MoveBuggy();
        ShowParticles();
        EndCycle();
    }

    private void StartCycle()
    {
        ProductionItem.gameObject.SetActive(false);
        SourceItem.localScale = Vector3.zero;
        SourceItem.gameObject.SetActive(true);
        SourceItem.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
    }

    private void MoveBuggy()
    {
        _buggy.position = StartSpawnPoint.position;
        _buggy.DOMove(_endPosition.position, _moveTime).SetDelay(_stayTime)
            .OnComplete(()=>ProductionItem.DOScale(Vector3.zero, _zoomTime));
    }

    private void ShowParticles()
    {
        DOVirtual.DelayedCall(_enableFxDelay, () =>
        {
            SetParticlesActive(true);
            SetRotatorActive(true);
        }).SetId(this);
        Timer.ExecuteWithDelay(() =>
        {
            SetParticlesActive(false);
            SetRotatorActive(false);
        }, _disableFxDelay);
    }

    private void EndCycle()
    {
        _buggy.DOMove(StartSpawnPoint.position, _returnTime).SetDelay(_stayTime * 2 + _moveTime);

        Timer.ExecuteWithDelay(()=>
        {
            SourceItem.gameObject.SetActive(false);
            ProductionItem.gameObject.SetActive(true);
            ProductionItem.localScale = Vector3.one;
        },_resourceSwapDelay);
    }
    
    private void SetRotatorActive(bool active)
    {
        foreach (var rotator in _rotators)
        {
            rotator.enabled = active;
        }
    }

    private void SetParticlesActive(bool active)
    {
        foreach (var particle in _particleSystems)
        {
            if(active)
                particle.Play();
            else
                particle.Stop();
        }
    }
}
