using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class ZoomListAnimation : MonoBehaviour
    {
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private float _zoomDelay;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _delayBetweenTargets;
        [SerializeField] private Ease _zoomEase;

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
            for (int i = 0; i < _targets.Count; i++)
            {
                _targets[i].transform.localScale = Vector3.zero;
                _targets[i].DOScale(1, _zoomDuration)
                    .SetDelay(_zoomDelay + i * _delayBetweenTargets)
                    .SetEase(_zoomEase)
                    .SetId(this);
            }
        }
        
    }
}