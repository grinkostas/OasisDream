using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Scripts.Animations
{
    public class RecyclerPuncher : MonoBehaviour
    {
        [SerializeField] private Recycler _recycler;
        [SerializeField] private Transform _model;
        [SerializeField] private Vector3 _startScale = Vector3.one;
        [SerializeField] private Vector3 _scaleDelta;
        [SerializeField] private Ease _zoomInEase;
        [SerializeField] private Ease _zoomOutEase;
        [SerializeField] private float _onePunchDuration;

        private void OnEnable()
        {
            _recycler.OnStartRecycle += OnStartRecycle;
            _recycler.OnEndRecycle += OnEndRecycle;
        }

        private void OnStartRecycle()
        {
            DOTween.Kill(this);
            _model.localScale = _startScale;
            StartPunching();
        }

        private void OnEndRecycle()
        {
            DOTween.Kill(this);
            _model.DOScale(_startScale, _onePunchDuration / 2).SetId(this);
        }

        [Button()]
        public void Restart()
        {
            OnStartRecycle();
        }

        private void StartPunching()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_model.DOScale(_startScale + _scaleDelta, _onePunchDuration / 2)).SetEase(_zoomInEase);
            sequence.Append(_model.DOScale(_startScale, _onePunchDuration / 2).SetEase(_zoomOutEase));
            sequence.SetId(this);
            sequence.SetLoops(-1);
        }
    }
}