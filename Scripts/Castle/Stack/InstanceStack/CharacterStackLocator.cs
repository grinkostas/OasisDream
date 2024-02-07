using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using NaughtyAttributes;
using StaserSDK.Stack;

public class CharacterStackLocator : MonoBehaviour
{
    [SerializeField] private InstanceStack _stack;
    [SerializeField] private Vector3 _columnOffsetDirection;
    [SerializeField] private float _columnsOffset;
    [SerializeField] private bool _customStackSettings;
    
    [SerializeField, ShowIf(nameof(_customStackSettings))] 
    private StackSettings _customSetting;

    public StackSettings Settings => _customStackSettings ? _customSetting : StackSettings.Value;
    public InstanceStack Stack => _stack;

    private void OnEnable()
    {
        _stack.TookItem += OnTakeItem;
    }
    
    private void OnDisable()
    {
        _stack.TookItem -= OnTakeItem;
    }

    private void Start()
    {
        Rebuild();
    }

    private void OnTakeItem(StackItemData itemData)
    {
        Rebuild(itemData.Target.transform);
    }

    private void Rebuild(Transform skipTransform = null)
    {
        for (int i = 0; i < _stack.SourceItems.Count; i++)
        {
            if(skipTransform == _stack.SourceItems[i].transform)
                continue;
            _stack.SourceItems[i].transform.localPosition = GetDelta(i);
        }
    }

    public Vector3 GetCurrentDelta()
    {
        return GetDelta(_stack.SourceItems.Count-1);
    }

    private Vector3 GetDelta(int index)
    {
        if(index == 0)
            return Vector3.zero;

        int columnsCount = GetColumn(index);
        Vector3 delta = columnsCount * _columnsOffset * _columnOffsetDirection;
        for (int i = columnsCount * Settings.MaxRowsCount; i < index; i++)
        {
            delta += _stack.SourceItems[i].StackSize.Size.y * Vector3.up;
        }

        return delta;
    }
    
    public int GetRow(StackItem item)
    {
        int index = _stack.SourceItems.IndexOf(item);
        return GetRow(index);
    }

    public int GetColumn(StackItem item)
    {
        int index = _stack.SourceItems.IndexOf(item);
        return GetColumn(index);
    }
    
    public int GetRow(int index)
    {
        if (index < Settings.MaxRowsCount)
            return index;
        int columnsCount = GetColumn(index);
        return index - columnsCount * Settings.MaxRowsCount;
    }

    public int GetColumn(int index)
    {
        return index / Settings.MaxRowsCount;
    }
}
