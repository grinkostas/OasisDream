using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ProgressibleBuilding : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private GameObject _baseModel;
    [SerializeField] private List<GameObject> _progressModels;
    [SerializeField] private float _zoomDuration;

    private int _currentIndex = -1;
    private bool _visibleBase = true;

    private void OnEnable()
    {
        foreach (var progressModel in _progressModels)
        {
            progressModel.SetActive(false);
        }

        Actualize(_buyZone.Progress);
        _buyZone.BuyProgressChangedDelayed.On(Actualize);
        _buyZone.Bought.On(OnBought);
    }

    private void OnDisable()
    {
        _buyZone.BuyProgressChangedDelayed.Off(Actualize);
        _buyZone.Bought.Off(OnBought);
    }

    private void OnBought()
    {
        Actualize(1.0f);
    }
    
    private void Actualize(float progress)
    {
        if(progress < 0.01f)
            return;
        
        int index = Mathf.CeilToInt(progress * (_progressModels.Count - 1));
        if (progress >= 0.95f)
            index = _progressModels.Count - 1;
        if(index <= _currentIndex)
            return;

        HideBase(index);
        ShowModels(index);

        _currentIndex = index;
    }

    private void HideBase(int index)
    {
        if (index <= 0 && _visibleBase == false)
            return;
        _baseModel.transform.localScale = Vector3.one;
        _baseModel.transform.DOScale(Vector3.zero, _zoomDuration)
            .OnComplete(() => _baseModel.SetActive(false));
        _visibleBase = false;
    }

    private void ShowModels(int index)
    {
        if(index > _progressModels.Count - 1 || index < 0)
            return;

        for (int i = 0; i < index+1; i++)
            ShowModel(i);
        
    }

    private void ShowModel(int index)
    {
        if(_progressModels[index].activeSelf)
            return;
        _progressModels[index].SetActive(true);
        _progressModels[index].transform.localScale = Vector3.zero;
        _progressModels[index].transform.DOScale(Vector3.one, _zoomDuration);
    }

    
}
