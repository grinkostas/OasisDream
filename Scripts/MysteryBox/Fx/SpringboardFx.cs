
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Scripts.MysteryBox
{
    public class SpringboardFx : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _springboard;
        [Header("Force")] 
        [SerializeField] private float _forceDuration;
        [SerializeField] private Ease _forceEase;
        [Header("Return")]
        [SerializeField] private Ease _returnEase;
        [SerializeField] private float _returnDelay;
        [SerializeField] private float _returnDuration;
        
        [Button()]
        public void Force()
        {
            if(DOTween.IsTweening(this))
                return;

            var sequence = DOTween.Sequence();
            sequence.Append(DOVirtual.Float(0, 100, _forceDuration, 
                value => _springboard.SetBlendShapeWeight(0, value))
                .SetEase(_forceEase));
            sequence.AppendInterval(_returnDelay);
            sequence.Append(DOVirtual.Float(100, 0, _returnDuration, 
                value => _springboard.SetBlendShapeWeight(0, value))
                .SetEase(_returnEase));
            sequence.SetId(this);
        }
    }
}