using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using StaserSDK.Interactable;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public class Helper : MonoBehaviour
{
    [SerializeField] private HelperHouse _helperHouse;
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private NavMeshAgentHandler _handler;
    [SerializeField] private StackableCharacter _stackableCharacter;
    public NavMeshAgentHandler Handler => _handler;
    public IStack Stack => _stackableCharacter.MainStack;
    public HelperHouse HelperHouse => _helperHouse;
    public Animator Animator => _animatorLinker.Animator;
}
