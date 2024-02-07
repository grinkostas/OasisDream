using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CheatCountView : MonoBehaviour
{
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _minusButton;
    [SerializeField] protected TMP_Text _countText;
    [SerializeField] private int _startValue;
    [SerializeField] protected int _step;

    public int CurrentValue { get; private set; }

    private void Awake()
    {
        ActualizeValue(_startValue);
    }

    private void OnEnable()
    {
        _plusButton.onClick.AddListener(OnPlusButtonClick);
        _minusButton.onClick.AddListener(OnMinusButtonClick);
    }

    private void OnDisable()
    {
        _plusButton.onClick.RemoveListener(OnPlusButtonClick);
        _minusButton.onClick.RemoveListener(OnMinusButtonClick);
    }

    private void OnPlusButtonClick()
    {
        Actualize(CurrentValue+_step);
        PlusButtonClick();
    }
    
    private void OnMinusButtonClick()
    {
        Actualize(CurrentValue-_step);
        MinusButtonClick();
    }

    protected virtual void PlusButtonClick()
    {}
    
    protected virtual void MinusButtonClick()
    {}
    
    private void Actualize(int value)
    {
        CurrentValue = value;
        _countText.text = CurrentValue.ToString();
        ActualizeValue(value);
    }
    
    protected virtual void ActualizeValue(int value)
    {
        int finalValue = Mathf.Max(0, value);
        CurrentValue = finalValue;
        _countText.text = CurrentValue.ToString();
    }
    
    
}
