using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class ResourceEarnView : MonoBehaviour
{
    [SerializeField] private ResourcePlaceGenerator _generator;
    [SerializeField] private TMP_Text _earnCount;
    [SerializeField] private Image _resourceIcon;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _fadeOutDelay;
    [SerializeField] private Bouncer _bouncer;

    [Inject] private Timer _timer;

    
    private int _currentEarned = 0;

    private void OnEnable()
    {
        _canvasGroup.alpha = 0;
        _generator.Spawned += OnSpawned;
    }

    private void Start()
    {
        _resourceIcon.sprite = _generator.ResourceIcon;
    }

    private void OnDisable()
    {
        _generator.Spawned -= OnSpawned;
    }

    private void OnSpawned(StackItem stackItem)
    {
        DOTween.Kill(this);
        _timer.ExecuteWithDelay(FadeOut, _fadeOutDelay).SetId(this);
        _canvasGroup.DOFade(1, _fadeDuration).SetId(this);
        _bouncer.Bounce();
        _currentEarned += stackItem.Amount;
        _earnCount.text = $"+{_currentEarned}";

    }

    private void FadeOut()
    {
        _canvasGroup.DOFade(0, _fadeDuration).OnComplete(() => _currentEarned = 0).SetId(this);
    }
}
