using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FurniceFx : RecycleAdditionalFx
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _middlePoint;
    [SerializeField] private float _halfPathMoveDuration;
    [SerializeField] private float _zoomTime;

    private Tweener _productItemTweener;
    
    protected override void ProductCycle()
    {
        MoveSourceItem();
        MoveProductItem();
    }

    private void MoveSourceItem()
    {
        SourceItem.position = StartSpawnPoint.position;
        SourceItem.localScale = Vector3.zero;
        SourceItem.gameObject.SetActive(true);
        SourceItem.DOScale(Vector3.one, _zoomTime);
        SourceItem.DOMove(_middlePoint.position, _halfPathMoveDuration).SetDelay(_zoomTime)
            .OnComplete(() => SourceItem.gameObject.SetActive(false));
    }

    private void MoveProductItem()
    {
        _productItemTweener?.Kill();
        ProductionItem.localScale = Vector3.zero;
        ProductionItem.position = _middlePoint.position;
        ProductionItem.gameObject.SetActive(true);
        float delay = _zoomTime + _halfPathMoveDuration;
        ProductionItem.DOScale(Vector3.one, _zoomTime).SetDelay(_halfPathMoveDuration);
        ProductionItem.DOMove(_endPoint.position, _halfPathMoveDuration).SetDelay(delay);
        delay += _halfPathMoveDuration;
        _productItemTweener = ProductionItem.DOScale(Vector3.zero, _zoomTime).SetDelay(delay)
            .OnComplete(() => ProductionItem.gameObject.SetActive(false));
    }
}
