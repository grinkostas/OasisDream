using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dreamteck;
using NaughtyAttributes;
using StaserSDK.Stack;
using TMPro;
using UnityEngine.UI;

public class ItemsTypeDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    public ItemType CurrentItemType { get; private set; } = ItemType.None;
    
    private void Awake()
    {
        Actualize();
    }

    private void OnEnable()
    {
        _dropdown.onValueChanged.AddListener(OnValueChanged);
    }
    
    private void OnDisable()
    {
        _dropdown.onValueChanged.RemoveListener(OnValueChanged);
    }


    [Button("Actualize Dropdown")]
    private void Actualize()
    {
        List<TMP_Dropdown.OptionData> options = new ();

        foreach (var itemType in Enum.GetNames(typeof(ItemType)))
        {
            options.Add(new TMP_Dropdown.OptionData(itemType));
        }

        _dropdown.options = options;
    }

    private void OnValueChanged(int index)
    {
        CurrentItemType = (ItemType)index;
    }
}
