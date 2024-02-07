using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ProgressiblePercentageView : MonoBehaviour
{
    [SerializeField] private TMP_Text _percentageText;
    [SerializeField, RequireInterface(typeof(IProgressible))] private GameObject _progressibleObject;
    [SerializeField] private bool _invert;

    private IProgressible _progressible;
    private IProgressible Progressible => _progressible ??= _progressibleObject.GetComponent<IProgressible>();


    private void OnEnable()
    {
        Progressible.ProgressChanged += OnProgressChanged;
    }
    
    private void OnDisable()
    {
        Progressible.ProgressChanged -= OnProgressChanged;
    }

    private void OnProgressChanged(float progress)
    {
        if(_invert)
            progress = 1 - progress;
        _percentageText.text = Mathf.CeilToInt(progress * 100) + "%";
    }
}
