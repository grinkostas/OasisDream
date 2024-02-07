using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public class RecyclerCompassBinder : MonoBehaviour
{
    [SerializeField] private Recycler _recycler;
    
    public ItemType ProductType => _recycler.ProductType;
    public ItemType SourceType => _recycler.SourceType;
}
