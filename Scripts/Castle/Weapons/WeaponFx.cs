using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using Zenject;
using Timer = StaserSDK.Utilities.Timer;

public class WeaponFx : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _weaponModel;
    [SerializeField] private float _zoomTime;
    [SerializeField] private bool _showTrail = true;
    [SerializeField, ShowIf(nameof(_showTrail))] private TrailRenderer _trailRenderer;
    [SerializeField, ShowIf(nameof(_showTrail))] private float _trailEnableDelay = 0.05f;
    [SerializeField, ShowIf(nameof(_showTrail))] private float _trailAdditionDisableTime = 0.05f;

    [Inject] private Timer _timer;
    
    private Tweener _scaleTweener = null;
    
    private void OnEnable()
    {
        _weapon.StartUsing += Show;
        _weapon.EndedUsing += Hide;
        Hide(_weapon);
    }
    
    private void OnDisable()
    {
        _weapon.StartUsing -= Show;
        _weapon.EndedUsing -= Hide;
    }
    
    private void Show(Weapon weapon)
    {
        _scaleTweener?.Kill();
        _scaleTweener = _weaponModel.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack);
        if(_showTrail)
            _timer.ExecuteWithDelay(EnableTrail, _trailEnableDelay);
    }

    private void EnableTrail()
    {
        if (_showTrail == false)
            return;
        _trailRenderer.emitting = true;
        DOVirtual.DelayedCall(_trailAdditionDisableTime, () =>
        {
            DOTween.Kill(this);
            _trailRenderer.emitting = false;
            DOVirtual.DelayedCall(_trailAdditionDisableTime, EnableTrail).SetId(this);
        }).SetId(this);
    }
    
    private void Hide(Weapon weapon)
    {
        _scaleTweener?.Kill();
        _scaleTweener = _weaponModel.DOScale(Vector3.zero, _zoomTime);
        
        if (_showTrail == false)
            return;
        DOTween.Kill(this);
        _trailRenderer.emitting = false;
    }
}
