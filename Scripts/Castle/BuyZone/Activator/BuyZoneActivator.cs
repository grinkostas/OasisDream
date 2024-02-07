using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NepixSignals;
using StaserSDK.Utilities;
using UnityEngine.UI;
using Zenject;

public class BuyZoneActivator : ABuyZoneActivator
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private List<GameObject> _activateObjects;
    [SerializeField] private List<GameObject> _disableObject;
    [SerializeField] private float _buyZoneZoomOutDuration;
    [SerializeField] private float _enableDelay;

    public BuyZone Zone => _buyZone;
    public override TheSignal Bought => _buyZone.Bought;

    public override bool IsBought() => _buyZone.IsBoughtCheck();
    public override bool Has(BuyZone zone)
    {
        return _buyZone == zone;
    }

    private void OnEnable()
    {
        _buyZone.Bought.On(EnableAll);
        if (_buyZone.IsBought())
        {
            _buyZone.Buy();
            return;
        }

        DisableAll();
    }

    private void OnDisable()
    {
        _buyZone.Bought.Off(EnableAll);
    }

    public override void EnableAll()
    {
        if(DOTween.IsTweening(this))
            return;

        var buyZoneTransform = _buyZone.transform;
        var zoomOutTweener = buyZoneTransform.DOScale(new Vector3(0.8f, 0, 0.8f), _buyZoneZoomOutDuration).SetId(this);
        buyZoneTransform.DOMove(buyZoneTransform.position + Vector3.down/2, _buyZoneZoomOutDuration).SetId(this);

        foreach (var disableObject in _disableObject)
            disableObject.transform.DOScale(new Vector3(0, 0, 0), _buyZoneZoomOutDuration).SetEase(Ease.InBack)
                .OnComplete(() => disableObject.SetActive(false)).SetId(this);
        
        foreach (var objectToActivate in _activateObjects)
            DOVirtual.DelayedCall(_enableDelay, () => objectToActivate.SetActive(true)).SetId(this);

        if (zoomOutTweener.active)
            zoomOutTweener.OnComplete(() => _buyZone.gameObject.SetActive(false));
        else
            _buyZone.gameObject.SetActive(false);
    }

    public override void Enable()
    {
        if (_buyZone.IsBought())
        {
            _buyZone.Buy();
            return;
        }
        DisableAll();
        _buyZone.gameObject.SetActive(true);
    }

    public override void DisableAll(bool disableZone = false)
    {
        foreach (var activateObject in _activateObjects)
            activateObject.SetActive(false);
        if(disableZone)
            _buyZone.gameObject.SetActive(false);
        
        
    }
    
}
