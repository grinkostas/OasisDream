using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;

public class SkinItemView : MonoBehaviour
{
    [SerializeField] private string _itemId;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;

    [Inject] private SkinManager _skinManager;

    public string ItemId => _itemId;
    
    private void OnEnable()
    {
        _skinManager.ChangedSkin += OnChangedSkin;
        Actualize();
    }

    public void Actualize()
    {
        SkinData currentSkinData = _skinManager.GetItemSkin(_itemId);
        _backgroundImage.color = _skinManager.GetColor(currentSkinData.Rarity);
        _iconImage.sprite = currentSkinData.Icon;
    }

    private void OnChangedSkin(ISkinnedChanger skinnedChanger, string skinId)
    {
        Actualize();
    }
    
}
