using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Scripts.Water;
using StaserSDK;
using StaserSDK.Interactable;

public class Player : MonoBehaviour
{
    [SerializeField] private EquippedCharacter _equippedCharacter;
    [SerializeField] private StackableCharacter _stackableCharacter;
    [SerializeField] private AnimatorLinker _animatorLinker;
    [SerializeField] private Transform _model;
    [SerializeField] private CharacterControllerMovement _movement;
    [SerializeField] private CharacterControllerMovement _characterControllerMovement;
    [SerializeField] private WaterBottle _waterBottle;
    [SerializeField] private GameObject _moveEffects;
    public Animator Animator => _animatorLinker.Animator;
    public Transform Model => _model;
    public MovementHandler Movement => _movement.Handler;
    public CharacterControllerMovement CharacterControllerMovement => _movement;
    public EquippedCharacter Equipment => _equippedCharacter;
    public StackableCharacter Stack => _stackableCharacter;
    public ModifiedValue<float> Speed => _characterControllerMovement.Speed;

    public WaterBottle WaterBottle => _waterBottle;
    public GameObject MoveEffects => _moveEffects;
}
