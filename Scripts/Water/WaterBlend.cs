using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.Water
{
    public class WaterBlend : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private float _cycleDuration;

        private void OnEnable()
        {
            Animate();
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        private void Animate()
        {
            DOTween.Kill(this);
            var sequence = DOTween.Sequence();
            sequence.Append(DOBlend(0, 0, 100));
            sequence.Join(DOBlend(1, 100, 0));
            sequence.Append(DOBlend(1, 0, 100));
            sequence.Join(DOBlend(0, 100, 0));
            sequence.OnComplete(Animate);
            sequence.SetId(this);
        }

        private Tween DOBlend(int index, float startValue, float endValue)
        {
            return DOVirtual.Float(startValue, endValue, _cycleDuration,
                value => _skinnedMeshRenderer.SetBlendShapeWeight(index, value));
        }
    }
}