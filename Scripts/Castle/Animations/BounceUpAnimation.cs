using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BounceUpAnimation : TweenAnimation<Transform>
{
    [Header("Position Change")]
    [SerializeField] private Ease _moveUpEase  = Ease.OutSine;
    [SerializeField] private Ease _moveDownEase = Ease.InSine;
    [SerializeField] private float _moveUpDuration = 0.35f;
    [SerializeField] private float _moveDownDuration = 0.25f;
    [SerializeField] private Vector3 _moveUpDelta = new (0f, 4f, 0f);
    [SerializeField] private Vector3 _startPosition;

    [Header("Scale Change")]
    [SerializeField] private Vector3 _startScale = new (0.8f, 1.3f, 0.8f);
    [SerializeField] private Vector3 _endScale = Vector3.one;
    [SerializeField] private float _zoomUpDuration = 0.35f;
    [SerializeField] private float _zoomDownDuration = 0.75f;
    [SerializeField] private Ease _scaleInEase = Ease.InCubic;
    [SerializeField] private Ease _scaleOutEase = Ease.OutBack;
    [Space] 
    [SerializeField] private float _delay;
    
    protected override Tweener Animate()
    {
        return Animate(transform);
    }
    
    protected override void OnRestore()
    {
        transform.localPosition =_startPosition;
        transform.localScale = Vector3.zero;
        DOTween.Kill(gameObject);
    }
    
    public override void Prepare(Transform source)
    {
        source.localPosition = _startPosition;
        source.localScale = Vector3.zero;
        DOTween.Kill(source.gameObject);
    }
    
    public override Tweener Animate(Transform source)
    {
        Bounce(source);
        return DOVirtual.Float(0, 0, 0, value => { });
    }

    private void Bounce(Transform source)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(source.DOScale(_startScale, _zoomUpDuration).SetEase(_scaleInEase));
        sequence.Join(source.DOLocalMove(_startPosition + _moveUpDelta, _moveUpDuration).SetEase(_moveUpEase));

        sequence.Append(source.DOScale(_endScale, _zoomDownDuration).SetEase(_scaleOutEase));
        sequence.Join(source.DOLocalMove(_startPosition, _moveDownDuration).SetEase(_moveDownEase));
        sequence.SetId(source.gameObject);
        sequence.SetDelay(_delay);
    }
}
