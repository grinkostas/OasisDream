using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;


public class BlendShapeAnimation : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private List<BlendSetting> _blendShapeIndexes;
    [SerializeField] private float _cycleDuration;
    [SerializeField] private Ease _cycleEase = Ease.OutQuad;

    [Serializable]
    private class BlendSetting
    {
        public int TargetBlend;
        public int BlendIndex;
    }
    private void OnEnable()
    {
        Blend();
    }

    private void Blend()
    {
        DOTween.Kill(this);
        for (int i = 0; i < _blendShapeIndexes.Count; i++)
        {
            var i1 = i;
            DOVirtual.Float(
                0, 
                _blendShapeIndexes[i1].TargetBlend,
                _cycleDuration,
                value =>
                {
                    _meshRenderer.SetBlendShapeWeight(_blendShapeIndexes[i1].BlendIndex, value);
                }).SetEase(_cycleEase).SetId(this).SetLoops(-1, LoopType.Yoyo);
        }
    }


    [Button]
    private void Restart()
    {
        DOTween.Kill(this);
        Blend();
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }
}
