using System;
using Cinemachine;
using DG.Tweening;
using GameCore.Scripts.Popups;
using GameCore.Scripts.Popups.PopupVariants;
using Haptic;
using StaserSDK.Interactable;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Balloon
{
    public class BalloonFx : MonoBehaviour
    {
        [SerializeField] private InteractableCharacterZone _interactableCharacterZone;
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private Transform _lookAtPoint;
        [SerializeField] private MoveFx _moveFx;
        [SerializeField] private ParticleSystem _fireParticle;
        [Header("Animations")]
        [SerializeField] private string _jumpParameter = "MarioJump";
        [SerializeField] private string _idleParaneter = "MarioJump";
        [SerializeField] private string _speedParameter = "MarioJump";
        [Header("Balloon")] 
        [SerializeField] private Transform _ballonTransform;
        [SerializeField] private Transform _ballonTargetPoint;
        [SerializeField] private float _balloonStartDelay;
        [SerializeField] private float _balloonFlyTime;
        [SerializeField] private Ease _flyEase;
        [Header("EndScreen")] 
        [SerializeField] private float _endScreenDelay;
        [SerializeField] private CinemachineVirtualCamera _camera;
        
        [Inject] public IHapticService Haptic { get; }
        [Inject] public Player Player { get; }
        [Inject] public PopupFactory PopupFactory { get; }


        private bool _playing = false;
        private void Awake()
        {
            _camera.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _interactableCharacterZone.OnEnter += OnEnter;
        }

        private void OnDisable()
        {
            _interactableCharacterZone.OnEnter -= OnEnter;
        }

        private void OnEnter(InteractableCharacter character)
        {
            if(_playing)
                return;
            _playing = true;
            Haptic.Selection();
            Player.MoveEffects.gameObject.SetActive(false);
            Player.Movement.DisableHandle(this);
            
            
            Player.Animator.SetTrigger(_jumpParameter);
            _moveFx.Move(Player.transform, _targetPoint, changeParent:true).OnComplete(()=>
                Player.Animator.SetTrigger(_idleParaneter));
            
            Player.Model.DOLookAt(_lookAtPoint.forward, _moveFx.Duration);
            
            
            Player.Animator.SetFloat(_speedParameter, 0);

            DOVirtual.DelayedCall(_balloonStartDelay, () =>
            {
                _fireParticle.Play();
                _camera.gameObject.SetActive(true);
                _ballonTransform.DOMove(_ballonTargetPoint.position, _balloonFlyTime).SetEase(_flyEase);
            });

            //DOVirtual.DelayedCall(_endScreenDelay, () => PopupFactory.Show<EndScreenPopup>());

        }
    }
}