using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.MysteryBox
{
    public class MysteryBoxDisposeFx : MonoBehaviour
    {
        [SerializeField] private MysteryBox _mysteryBox;
        [SerializeField] private List<Transform> _componentsToScaleOut;
        [SerializeField] private Ease _hideEase;
        [SerializeField] private float _zoomOutDuration;

        private void OnEnable()
        {
            _mysteryBox.Disabled.On(OnDisabled);
        }

        private void OnDisable()
        {
            _mysteryBox.Disabled.Off(OnDisabled);
        }

        private void OnDisabled()
        {
            foreach (var component in _componentsToScaleOut)
            {
                component.DOScale(Vector3.zero, _zoomOutDuration).SetEase(_hideEase);
            }
        }
    }
}