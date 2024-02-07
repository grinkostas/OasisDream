using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public class SoftCurrencyPresenter : MonoBehaviour
{
    [SerializeField] private SoftCurrencyDisplay _currencyDisplay;

    public IStack SoftStack => _currencyDisplay.Player.Stack.SoftCurrencyStack;
    
    private void OnEnable()
    {
        Actualize();
        SoftStack.CountChanged += OnCountChanged;
    }

    private void OnDisable()
    {
        SoftStack.CountChanged -= OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        Actualize();
    }

    private void Actualize()
    {
        if(SoftStack.Items[ItemType.Diamond].Value > 0)
            _currencyDisplay.gameObject.SetActive(true);
        else 
            _currencyDisplay.gameObject.SetActive(false);
    }
}
