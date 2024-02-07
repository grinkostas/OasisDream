using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;

public class PhasedBuilding : MonoBehaviour
{
    [SerializeField] private Transform _wrapper;
    [SerializeField] private float _scaleMultiplayer;
    [SerializeField] private float _punchDuration;
    [SerializeField] private List<GameObject> _buildPhases;

    private int _currentPhase = -1;
    private GameObject _currentPhaseObject = null;
    
    private void OnEnable()
    {
        Build();
    }


    private void Build()
    {
        foreach (var buildPhase in _buildPhases)
        {
            buildPhase.SetActive(false);
        }
        NextPhase();
    }
    
    private void NextPhase()
    {
        _currentPhase++;
        if(_currentPhase >= _buildPhases.Count)
            return;
        
        _wrapper.DOScale(_scaleMultiplayer, _punchDuration / 2).OnComplete(() =>
        {
            if(_currentPhaseObject != null)
                _currentPhaseObject.SetActive(false);
        
            _currentPhaseObject = _buildPhases[_currentPhase];
            _currentPhaseObject.SetActive(true);
            
            _wrapper.DOScale(Vector3.one, _punchDuration / 2).SetEase(Ease.OutBack)
                .OnComplete(NextPhase);
        });
    }
    
    
}
