using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ISellable
{
    public bool IsSellable { get; }
    public int SellPrice { get; }
    public Sprite Icon { get; }
}
