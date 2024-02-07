using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.Water
{
    public class Hose : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _hose;
        [SerializeField] private int _blendShapesCount;
        [SerializeField] private float _oneBlendDurarion;

        public void UseWater()
        {
            DOTween.Kill(this);
            var sequence = DOTween.Sequence().SetId(this);
            sequence.Append(DoBlendShape(_blendShapesCount-1, 0, 100));
            sequence.Append(DoBlendShape(_blendShapesCount-1, 100, 0));
            for (int i = _blendShapesCount-2; i >= 0; i--)
            {
                sequence.Join(DoBlendShape(i, 0, 100));
                sequence.Append(DoBlendShape(i, 100, 0));
            }
            sequence.OnComplete(UseWater);
        }
        
        public void ReceiveWater()
        {
            DOTween.Kill(this);
            var sequence = DOTween.Sequence().SetId(this);
            sequence.Append(DoBlendShape(0, 0, 100));
            sequence.Append(DoBlendShape(0, 100, 0));
            for (int i = 1; i < _blendShapesCount; i++)
            {
                sequence.Join(DoBlendShape(i, 0, 100));
                sequence.Append(DoBlendShape(i, 100, 0));
            }
            sequence.OnComplete(ReceiveWater);
        }
        
        public void ResetBlendShape()
        {
            DOTween.Kill(this);
            for (int i = 0; i < _blendShapesCount; i++)
            {
                DoBlendShape(i, _hose.GetBlendShapeWeight(i), 0).SetId(this);
            }
        }

        private Tween DoBlendShape(int blendShapeIndex, float startValue, float endValue)
        {
            return DOVirtual.Float(startValue, endValue, _oneBlendDurarion,
                value => _hose.SetBlendShapeWeight(blendShapeIndex, value));
        }
    }
}