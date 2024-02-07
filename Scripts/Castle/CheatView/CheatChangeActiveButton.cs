using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Scripts.Castle.CheatView.Logic;
using UnityEngine.UI;
using Zenject;

public class CheatChangeActiveButton : CheatButtonBase
{
    [SerializeField] private List<int> _attachedObjectIds;
    [SerializeField] private bool _onlyDisable;
    [SerializeField] private bool _onlyEnable;

    [Inject] public CheatContainer CheatContainer { get; }
    
    private bool _currentActive = true;

    public override void OnButtonClicked()
    {
        _currentActive = !_currentActive;
        bool targetActive = _currentActive;
        if (_onlyDisable) targetActive = false;
        if(_onlyEnable) targetActive = true;
        foreach (var attachedObjectId in _attachedObjectIds)
        {
            Debug.Log($"Set {CheatContainer.StoredObjects[attachedObjectId].name} active {targetActive}");
            CheatContainer.StoredObjects[attachedObjectId].SetActive(targetActive);
        }
    }
}
