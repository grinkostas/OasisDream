using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Polybrush;

[RequireComponent(typeof(Slider))]
public class SliderScroller : UIScroller
{
    private Slider _slider;
    private bool _implementedSlider = false;
    private Slider Slider 
    {
        get
        {
            if (_implementedSlider == false)
            {
                _slider = GetComponent<Slider>();
                _implementedSlider = true;
            }

            return _slider;
        }
    }
    protected override GameObject Target => Slider.handleRect.gameObject;

    protected override void OnHorizontalScroll(float delta)
    {
        Slider.value = Mathf.Clamp01(Slider.value + delta);
    }
    
}
