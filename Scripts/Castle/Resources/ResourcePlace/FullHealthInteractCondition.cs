using System;
using StaserSDK.Interactable;
using UnityEngine;


public class FullHealthInteractCondition : InteractCondition
{
    [SerializeField] private Destructible _destructible;
    public override bool CanInteract(InteractableCharacter character)
    {
        return _destructible.Health == _destructible.MaxHealth;
    }
}
