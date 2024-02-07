using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace GameCore.Scripts.Animations
{
    public class MoveAnimation : TweenAnimation<Transform>
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _startPostion;
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private bool _local;
        [SerializeField] private float _moveTime;
        [SerializeField] private Ease _moveEase;
        [SerializeField] private float _delay;

        protected override Tweener Animate()
        {
            return Animate(_target);
        }

        protected override void OnRestore()
        {
            Prepare(_target);
        }

        public override void Prepare(Transform source)
        {
            if (_local)
                source.localPosition = _startPostion;
            else
                source.position = _startPostion;
        }

        public override Tweener Animate(Transform source)
        {
            if (_local == false)
                return source.DOMove(_endPosition, _moveTime).SetEase(_moveEase).SetDelay(_delay);
            return source.DOLocalMove(_endPosition, _moveTime).SetEase(_moveEase).SetDelay(_delay);
        }
    }
}