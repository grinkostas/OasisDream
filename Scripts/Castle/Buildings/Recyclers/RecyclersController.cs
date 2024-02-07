using System.Collections.Generic;
using StaserSDK.Stack;
using UnityEngine;

public class RecyclersController
{
    private Dictionary<string, Recycler> _recyclers = new();

    public List<Recycler> Recyclers { get; private set; } = new();

    public void Add(string id, Recycler recycler)
    {
        _recyclers[id] = recycler;
        Recyclers.Add(recycler);
    }

    public void Remove(string id)
    {
        if (_recyclers.ContainsKey(id) == false)
            return;

        var recycler = _recyclers[id];
        Recyclers.Add(recycler);
        _recyclers.Remove(id);
    }

    public bool TryGet(string id, out Recycler recycler)
    {
        recycler = null;
        if (_recyclers.ContainsKey(id) == false)
            return false;
        recycler = _recyclers[id];
        return true;
    }

    public bool TryGetByProductType(ItemType targetType, out Recycler recycler)
    {
        recycler = Recyclers.Find(x => x.ProductType == targetType);
        return Recyclers.Has(x=> x.ProductType == targetType);
    }
    
    public bool TryGetBySourceType(ItemType targetType, out Recycler recycler)
    {
        recycler = Recyclers.Find(x => x.SourceType == targetType);
        return Recyclers.Has(x => x.SourceType == targetType);
    }
}
