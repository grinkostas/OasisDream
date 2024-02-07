using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SkinnedItemData
{
    [SerializeField] private string _itemId;
    [SerializeField] private List<SkinData> _skins;

    public string ItemId => _itemId;
    public List<SkinData> Skins => _skins;

}
