using DG.Tweening;
using StaserSDK.SaveProperties.Api;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class BlendShapeAnimation : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private float _startValue;
        [SerializeField] private float _endValue;
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _animationDelay;
        private void OnEnable()
        {
            DOVirtual.Float(_startValue, _endValue, _animationDuration,
                    value => _meshRenderer.SetBlendShapeWeight(0, value))
                .SetDelay(_animationDelay)
                .SetId(this);
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }
    }
}