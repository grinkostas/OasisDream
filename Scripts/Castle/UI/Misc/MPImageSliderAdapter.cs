using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MPUIKIT;
using UnityEngine.UI;

public class MPImageSliderAdapter : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private MPImage _image;

    [NaughtyAttributes.Button("Actualize")]
    private void Actualize()
    {
        _image.fillAmount = _slider.value;
    }
    
    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    
    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        _image.fillAmount = value;
    }
}
