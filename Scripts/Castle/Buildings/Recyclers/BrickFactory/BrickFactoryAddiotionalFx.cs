using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BrickFactoryAddiotionalFx : RecycleAdditionalFx
{
    [SerializeField] private Transform _hammer;
    [SerializeField] private Transform _hammerStartPoint;
    [SerializeField] private Transform _hammerDestinationPoint;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _actionTime;
    [SerializeField] private float _actionDelay;
    
    protected override void ProductCycle()
    {
        DOVirtual.DelayedCall(_actionDelay, () =>
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_hammer.DOMove(_hammerDestinationPoint.position, _actionTime).OnComplete(()=>_particle.Play()));
            sequence.Append(_hammer.DOMove(_hammerStartPoint.position, _actionTime));
            sequence.SetId(this);
        }).SetId(this);
    }
}
