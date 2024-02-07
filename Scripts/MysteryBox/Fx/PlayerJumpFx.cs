using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox
{
    public class PlayerJumpFx : MonoBehaviour
    {
        [Header("Jump")] 
        [SerializeField] private string _jumpAnimationParameter = "MarioJump";
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private Ease _jumpEase;
        [Header("Floating")] 
        [SerializeField] private float _floatingDelay;
        [SerializeField] private float _floatingDuration;
        [SerializeField] private Ease _floatingEase;
        
        [Inject] public Player Player { get; }
        
        [Button()]
        public void Jump()
        {
            if(DOTween.IsTweening(this))
                return;
            Player.Animator.SetTrigger(_jumpAnimationParameter);
            DOJump();
        }

        private void DOJump()
        {
            Vector3 startPosition = Player.Model.position;
            Transform model = Player.Model;

            var sequence = DOTween.Sequence();
            sequence.Append(model.DOMoveY(startPosition.y + _jumpHeight, _jumpDuration).SetEase(_jumpEase));
            sequence.AppendInterval(_floatingDelay);
            sequence.Append(model.DOMoveY(startPosition.y, _floatingDuration).SetEase(_floatingEase));
            sequence.SetId(this);
        }

    }
}