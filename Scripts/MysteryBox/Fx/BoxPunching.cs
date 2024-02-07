using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.MysteryBox
{
    public class BoxPunching : MonoBehaviour
    {
        [SerializeField] private Transform _box;
        [Header("Models")]
        [SerializeField] private GameObject _closedModel;
        [SerializeField] private GameObject _opennedModel;
        [SerializeField] private float _modelSwapDelay;
        [Space]
        [SerializeField] private float _scalePunch;
        [SerializeField] private float _jumpPunch;
        [SerializeField] private float _punchDurarion;
        [SerializeField] private float _punchDelay;

        private void Awake()
        {
            _closedModel.SetActive(true);
            _opennedModel.SetActive(false);
        }

        public void Punch()
        {
            if(DOTween.IsTweening(this))
                return;

            DOVirtual.DelayedCall(_modelSwapDelay, () =>
            {
                _closedModel.SetActive(false);
                _opennedModel.SetActive(true);
            }).SetId(this);
            
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(_punchDelay);
            sequence.Append(_box.DOPunchPosition(Vector3.up * _jumpPunch, _punchDurarion, 2));
            sequence.Join(_box.DOPunchScale(Vector3.one * _scalePunch, _punchDurarion, 2));
            sequence.SetId(this);
        }
        
    }
}