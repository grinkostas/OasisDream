using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using StaserSDK.Stack;

[CreateAssetMenu]
public class SkinData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private SkinRarity _skinRarity;
    [SerializeField] private bool _available;
    [SerializeField, HideIf(nameof(_available))] private CostData _price;

    public string Id => _id;
    public bool Available => _available || ES3.Load(_id, false);
    public CostData Price => _price;
    public Sprite Icon => _icon;

    public SkinRarity Rarity => _skinRarity;

    public void Buy()
    {
        ES3.Save(_id, true);
    }
}
