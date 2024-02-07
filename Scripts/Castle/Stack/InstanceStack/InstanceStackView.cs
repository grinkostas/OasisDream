using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class InstanceStackView : MonoBehaviour
{
    [SerializeField] private InstanceStack _instanceStack;
    [SerializeField] private View _modelView;
    [SerializeField] private TMP_Text _countText;
    private void OnEnable()
    {
        _instanceStack.CountChanged += OnCountChanged;
    }

    private void OnDisable()
    {
        _instanceStack.CountChanged -= OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        if (count < _instanceStack.MaxVisibleInstanceCount)
        {
            _modelView.Hide();
            return;
        }
        _modelView.Show();
        _countText.text = count.ToString();
    }
}
