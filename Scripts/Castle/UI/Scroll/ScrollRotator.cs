using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollRotator : UIScroller
{
    [SerializeField] private RectTransform _scrollArea;
    [SerializeField] private Transform _rotateModel;
    [SerializeField] private Vector3 _scrollAxis;
    protected override GameObject Target => _scrollArea.gameObject;

    protected override void OnHorizontalScroll(float delta)
    {
        _rotateModel.localRotation = Quaternion.Euler(_rotateModel.localRotation.eulerAngles + (_scrollAxis * delta));
    }
    
}
