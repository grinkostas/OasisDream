using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class PlayerModifierBoosterPopup : BoosterPopup
{
    [SerializeField] private string _id = "StackModifier";
    [Inject, UsedImplicitly] public ModifiersController modifiersController { get; }
    protected override void TakeReward()
    {
        modifiersController.UseModifier(Modifiers.GetModifier(_id));
    }
}
