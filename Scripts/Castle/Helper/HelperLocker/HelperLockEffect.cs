using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Utilities;
using Zenject;

public class HelperLockEffect : MonoBehaviour
{
    [SerializeField] private HelperLocker _helperLocker;
    [SerializeField] private ParticleSystem _particle;

    [SerializeField, ShowIf(nameof(HaveAnimator)), AnimatorParam(nameof(Animator))]
    private string _waitParameter;
    
    [SerializeField, ShowIf(nameof(HaveAnimator)), AnimatorParam(nameof(Animator))]
    private string _releaseParameter;

    [SerializeField] private float _enableHandleDelay;

    [Inject] private Timer _timer;

    private Animator Animator => _helperLocker.Helper.Animator;
    private bool HaveAnimator => _helperLocker != null && _helperLocker.Helper != null && Animator != null;
    
    private void OnEnable()
    {
        Lock();
        if (_helperLocker.IsLocked)
        {
            Unlock();
            return;
        }
        _helperLocker.Released += Unlock;
    }

    private void OnDisable()
    {
        _helperLocker.Released -= Unlock;
    }

    private void Lock()
    {
        _helperLocker.Helper.Handler.enabled = false;
        Animator.SetTrigger(_waitParameter);
    }
    
    private void Unlock()
    {
        Animator.SetTrigger(_releaseParameter);
        _particle.Play();
        _timer.ExecuteWithDelay(() =>
        {
            _helperLocker.Helper.Handler.enabled = true;
        }, _enableHandleDelay);
    }
}
