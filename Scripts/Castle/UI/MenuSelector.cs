using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private List<STuple<View, SelectableButton>> _views;

    private int _currentView = 0;

    private void OnEnable()
    {
        foreach (var view in _views)
        {
            view.Value2.Button.onClick.AddListener(()=>Show(_views.IndexOf(view)));
        }
        Show(_currentView);
    }

    private void OnDisable()
    {
        foreach (var view in _views)
        {
            view.Value2.Button.onClick.RemoveAllListeners();
        }
    }

    public void Show(int index = 0)
    {
        if (index >= _views.Count)
            return;
        _currentView = index;
        foreach (var view in _views)
        {
            view.Value1.Hide();
            view.Value2.Deselect();
        }

        _views[index].Value1.Show();
        _views[index].Value2.Select();
    }
}
