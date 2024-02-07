using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class TileSwapModel : MonoBehaviour
    {
       
        [SerializeField] private GameObject _animatedTile;
        [SerializeField] private GameObject _batchedTile;
        [SerializeField] private float _delay;

        private void OnEnable()
        {
            DOTween.Kill(this);
            _animatedTile.gameObject.SetActive(true);
            _batchedTile.gameObject.SetActive(false);
            DOVirtual.DelayedCall(_delay, () =>
            {
                _animatedTile.gameObject.SetActive(false);
                _batchedTile.gameObject.SetActive(true);
            }).SetId(this);
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }
    }
}