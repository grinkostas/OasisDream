using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Haptic;
using NaughtyAttributes;
using StaserSDK;
using StaserSDK.Utilities;
using Zenject;

public class DestructibleModel : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private float _changeModelDelay;
    [SerializeField] private List<GameObject> _models;
    [SerializeField] private GameObject _destroyedModel;
    [Header("Collider")] 
    [SerializeField] private bool _haveCollider;
    [SerializeField, ShowIf(nameof(_haveCollider))] private Collider _collider;

    [Inject] private Timer _timer;

    private GameObject _currentModel;

    private void OnEnable()
    {
        _destructible.HealthChanged += OnHealthChanged;
        ActualizeModel();
    }

    private void OnDisable()
    {
        _destructible.HealthChanged -= OnHealthChanged;
    }
    
    private void Restore()
    {
        if(_models.Count > 0)
            _currentModel = _models[^1];
        foreach (var model in _models)
            model.gameObject.SetActive(false);

        _destroyedModel.gameObject.SetActive(false);
        _currentModel.gameObject.SetActive(true);
        if (_haveCollider && _collider != null)
        {
            _collider.enabled = true;
        }
    }

    private void OnHealthChanged()
    {
        if(_models.Count == 0)
            return;
        
        ActualizeModel();
    }

    private void ActualizeModel()
    {
        if (_destructible.Health == _destructible.MaxHealth)
        {
            Restore();
            return;
        }
        
        if (_destructible.Health <= 0)
        {
            if (_haveCollider && _collider != null)
                _collider.enabled = false;
            SwapModel(_destroyedModel);
            return;
        }
        
        float modelDelta = _destructible.MaxHealth / (float)_models.Count;
        int index = (int)(_destructible.Health / modelDelta-1);
        var modelToSwap = _models[index];
        
        if(_currentModel != modelToSwap)
            SwapModel(modelToSwap);
    }

    private void SwapModel(GameObject model)
    {
        _timer.ExecuteWithDelay(() =>
        {
            _currentModel.SetActive(false);
            model.SetActive(true);
            _currentModel = model;
        }, _changeModelDelay);
    }

   
}
