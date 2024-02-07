using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Interactable;
using StaserSDK.Stack;

[RequireComponent(typeof(Collider))]
public class HelperHouseStackGiver : ZoneBase
{
    [SerializeField] private StackBase _helperHouseStack;
    [SerializeField] private float _takeItemDelay;

    public List<string> log = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out StackableCharacter character))
        {
            GiveItem(character);
            OnEnter?.Invoke(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out StackableCharacter character))
        {
            DOTween.Kill(character);
            OnExit?.Invoke(character);
        }
    }

    private void GiveItem(StackableCharacter character)
    {
        DOTween.Kill(character);
        DOVirtual.DelayedCall(_takeItemDelay, ()=> GiveItem(character)).SetId(character);

        var stack = character.MainStack;
        if (_helperHouseStack.ItemsCount == 0 || stack.ItemsCount >= stack.MaxSize)
        {
            DOTween.Kill(character);
            return;
        }

        if (_helperHouseStack.TryTake(ItemType.Any, out StackItem item, character.transform) == false)
            return;
        
        log.Add($"Took {item.Type}");
        if(item.IsClaimed == false)
            item.Claim();

        item.gameObject.SetActive(true);
        item.transform.localScale = Vector3.one;
        stack.TryAdd(item);
        OnInteract?.Invoke(character);
    }
    
}