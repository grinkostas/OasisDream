using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RarityColor
{
    [SerializeField] private SkinRarity _rarity;
    [SerializeField, ColorUsage(true)] private Color _color;

    public SkinRarity Rarity => _rarity;
    public Color Color => _color;
}
