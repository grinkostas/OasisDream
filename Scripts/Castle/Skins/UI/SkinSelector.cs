using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class SkinSelector : View, ISkinnedChanger
{
    [SerializeField] private SkinSelectView _skinSelectView;

    [Inject] private SkinManager _skinManager;

    public override bool IsHidden => enabled;
    public string ItemId => _skinSelectView.ItemId;
    private void OnEnable()
    {
        _skinSelectView.Selected += OnSelected;
    }
    
    private void OnDisable()
    {
        _skinSelectView.Selected -= OnSelected;
    }

    private void OnSelected(SkinData skinData)
    {
        _skinManager.ChangeSkin(this, skinData.Id);
        _skinSelectView.Hide();
    }


    public override void Show()
    {
        if(_skinManager.GetAvailableSkins(ItemId).Count <= 1)
            return;
        _skinSelectView.Show();
    } 
    public override void Hide() => _skinSelectView.Hide();
}
