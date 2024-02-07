using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Zenject;

public class SkinnedItem : MonoBehaviour, ISkinnedChanger
{
    [SerializeField] private string _itemId;
    [SerializeField] private List<Skin> _skins;
    [SerializeField] private Skin _defaultSkin;

    [Inject] private SkinManager _skinManager;
    
    private Skin _currentSkin;
    private Skin _selectedSkin;

    protected List<Skin> Skins => _skins;
    protected Skin DefaultSkin => _defaultSkin;
    public string ItemId => _itemId;

    private void OnEnable()
    {
        EquipSavedSkin();
        _skinManager.ChangedSkin += OnSkinChange;
        OnEnableInternal();
    }

    protected virtual void OnEnableInternal()
    {
    }

    private void OnSkinChange(ISkinnedChanger skinnedChanger, string skinId)
    {
        if((object)skinnedChanger == (object)this || skinnedChanger.ItemId != ItemId)
            return;
        EquipSkin(skinId);
    }

    [Inject]
    public void OnInject()
    {
        _skinManager.Add(this);
    }

    public void EquipSavedSkin()
    {
        if(_currentSkin != null && _currentSkin.Id == GetDefaultSkinId())
            return;
        
        foreach (var skin in _skins)
            skin.Deselect();
        
        EquipSkin(GetDefaultSkinId());
    }

    protected virtual string GetDefaultSkinId() => _defaultSkin.Id;

    public void SelectSkin(string id)
    {
        if(_currentSkin != null)
            _currentSkin.Deselect();
        if(_selectedSkin != null)
            _selectedSkin.Deselect();
        
        _selectedSkin = _skins.Find(x => x.Id == id);
        _selectedSkin.Select();
    }

    public void ResetSelection()
    {
        if(_selectedSkin != null)
            _selectedSkin.Deselect();

        if (_currentSkin != null)
            _currentSkin.Select();
        else
            EquipSavedSkin();
    }

    public void EquipSkin(string id)
    {
        var targetSkin = _skins.Find(x => x.Id == id);
        
        if(targetSkin == null)
            return;
        
        if(targetSkin.Available == false)
            return;

        if(_currentSkin != null)
            _currentSkin.Deselect();
        targetSkin.Select();
        _currentSkin = targetSkin;
    }

}
