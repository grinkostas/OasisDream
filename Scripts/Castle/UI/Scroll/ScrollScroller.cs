using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollScroller : UIScroller
{
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private Scrollbar _scrollbar;
    protected override GameObject Target => _viewport.gameObject;

    protected override bool NeedToUpdateGUIExternal()
    {
        return EventSystem.current.currentSelectedGameObject == null;
    }
    
    protected override void OnVerticalScroll(float delta)
    {
        _scrollbar.value = Mathf.Clamp01(_scrollbar.value + delta);
    }
}
