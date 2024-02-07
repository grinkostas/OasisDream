using DG.Tweening;
using StaserSDK.Stack;
using UnityEngine;


public class BakeryFx : RecycleAdditionalFx
{
    [Header("Bakery")]
    [SerializeField] private Transform _bakeryModel;
    [SerializeField] private float _punchDuration;
    [SerializeField] private int _punchCount;
    [SerializeField] private Vector3 _punch;
    [Space]
    [Header("Resource Move")]
    [SerializeField] private Transform _middlePoint;
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private Ease _moveEase;
    [SerializeField] private float _waitInMiddlePoint;
    [Space] 
    [Header("ResourceZoom")]
    [SerializeField] private float _zoomInTime;
    [SerializeField] private Ease _zoomInEase;
    [SerializeField] private float _zoomOutTime;
    [SerializeField] private Ease _zoomOutEase;
    [Header("Fx")] 
    [SerializeField] private ParticleSystem _productionPartice;
    [SerializeField] private float _particlePlayDelay;
    
   private float HalfPathDuration => (CycleDuration - _waitInMiddlePoint) / 2;

    protected override void OnStartRecycle()
    {
        DOTween.Kill(this);
        OnEndRecycle();
    }
    
    protected override void ProductCycle()
    {
        SourceItem.localScale = Vector3.zero;
        SourceItem.transform.position = StartSpawnPoint.position;

        SourceItem.gameObject.SetActive(true);
        ProductionItem.gameObject.SetActive(false);

        var sequence = DOTween.Sequence();
        sequence.Append(SourceItem.DOMove(_middlePoint.position, HalfPathDuration).SetEase(_moveEase).OnComplete(()=>
        {
            SourceItem.gameObject.SetActive(false);
            ProductionItem.transform.position = _middlePoint.position;
            ProductionItem.transform.localScale = Vector3.one;
            ProductionItem.gameObject.SetActive(true); 
        }));
        sequence.Join(SourceItem.DOScale(1, _zoomInTime).SetEase(_zoomInEase));
        sequence.AppendInterval(_waitInMiddlePoint);
        sequence.Join(_bakeryModel.DOPunchScale(_punch, _punchDuration, _punchCount));
        sequence.Append(ProductionItem.DOMove(_destinationPoint.position, HalfPathDuration).SetEase(_moveEase));
        sequence.Join(ProductionItem.DOScale(0, _zoomOutTime).SetEase(_zoomOutEase).SetDelay(HalfPathDuration-_zoomOutTime));
        sequence.SetId(this);
 
        DOVirtual.DelayedCall(_particlePlayDelay, _productionPartice.Play);

    }
    protected override void OnEndRecycle()
    {
        SourceItem.gameObject.SetActive(false);
        ProductionItem.gameObject.SetActive(false);
        _productionPartice.Stop();
    }
}
