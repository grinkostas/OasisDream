using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(RectTransform))]
public class ButtonClickZoom : MonoBehaviour
{
    [SerializeField] private float _zoomDuration = 0.25f;
    [SerializeField] private float _punch = 0.15f;
    [SerializeField] private bool _externRect;
    [SerializeField, ShowIf(nameof(_externRect))] private RectTransform _customRect;
    
    private Button _button;
    private RectTransform _rect;

    private Tweener _punchTweener;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _rect = _externRect ? _customRect : GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if(_punchTweener is { active: true })
            return;
        _punchTweener = _rect.DOPunchScale(_punch * Vector3.one, _zoomDuration, 2);
    }
}
