using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK.Interactable;
using StaserSDK.Stack;

[RequireComponent(typeof(Collider))]
public class HelperStackTaker : MonoBehaviour
{
    [SerializeField] private Transform _takePoint;
    [SerializeField] private StackBase _helperHouseStack;
    [SerializeField] private float _takeItemDelay;

    public List<string> log = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StackableCharacter character))
        {
            TakeHelperItems(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out StackableCharacter character))
        {
            DOTween.Kill(character);
        }
    }

    private void TakeHelperItems(StackableCharacter character)
    {
        DOTween.Kill(character);
        DOVirtual.DelayedCall(_takeItemDelay, ()=> TakeHelperItems(character)).SetId(character);

       
        if (character.MainStack.ItemsCount == 0)
        {
            DOTween.Kill(character);
            return;
        }
        log.Add("count > 0");
        if(_helperHouseStack.ItemsCount >= _helperHouseStack.MaxSize)
            return;
        
        log.Add("House stack have free places");
        var stack = character.MainStack;
        
        if (stack.TryTake(ItemType.Any, out StackItem item, _takePoint) == false)
            return;
        log.Add($"Took {item.Type}");
        if(item.IsClaimed == false)
            item.Claim();
        
        _helperHouseStack.Add(item);
    }
    
}
