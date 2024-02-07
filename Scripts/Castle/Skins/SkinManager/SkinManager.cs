using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Events;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private List<SkinnedItemData> _skinnedItemsData;
    [SerializeField] private List<RarityColor> _rarityColors;

    private List<SkinnedItem> _skinnedItems = new();
    public event UnityAction<ISkinnedChanger, string> ChangedSkin;

    
    public void ChangeSkin(ISkinnedChanger skinnedChanger, string skinId)
    {
        ES3.Save(skinnedChanger.ItemId, skinId);
        ChangedSkin?.Invoke(skinnedChanger, skinId);
    }

    public void Add(SkinnedItem skinnedItem)
    {
        if(_skinnedItems.Contains(skinnedItem))
            return;
        _skinnedItems.Add(skinnedItem);
    }

    public SkinnedItem GetItemBuyId(string itemId)
    {
        return _skinnedItems.Find(x => x.ItemId == itemId);
    }

    public List<SkinData> GetAvailableSkins(string itemId)
    {
        var data = _skinnedItemsData.Find(x => x.ItemId == itemId);
        if (data == null)
            return new List<SkinData>();
        return data.Skins.FindAll(x => x.Available);
    }

    public Color GetColor(SkinRarity rarity)
    {
        return _rarityColors.Find(x => x.Rarity == rarity).Color;
    }

    public SkinData GetSkin(string itemId, string skinId)
    {
       return _skinnedItemsData.Find(x => x.ItemId == itemId).Skins.Find(x=> x.Id == skinId);
    }

    public SkinData GetItemSkin(string itemId)
    {
        if (ES3.KeyExists(itemId))
            return GetSkin(itemId, ES3.Load<string>(itemId));

        return _skinnedItemsData.Find(x => x.ItemId == itemId).Skins.Find(x => x.Available);
    }

    public string GetSkinUser(string skinId)
    {
        foreach (var skinnedItemData in _skinnedItemsData)
        {
            if (skinnedItemData.Skins.Has(x => x.Id == skinId))
                return skinnedItemData.ItemId;
        }

        return "";
    }

    public List<SkinData> GetAllSkins()
    {
        var skins = new List<SkinData>();
        foreach (var skinnedItemData in _skinnedItemsData)
        {
            skins.AddRange(skinnedItemData.Skins);
        }

        skins = skins.DistinctBy(x=>x.Id).ToList();

        return skins;
    }
}
