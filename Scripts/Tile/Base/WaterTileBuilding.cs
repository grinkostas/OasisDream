using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    [RequireComponent(typeof(Collider))]
    public class WaterTileBuilding : MonoBehaviour
    {
        [SerializeField] private float _enableDelay = 0.25f;
        [SerializeField] private bool _disabledGrass = true;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Tile tile))
            {
                Enable(tile);
            }
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        private void Enable(Tile tile)
        {
            if(_disabledGrass)
                DOVirtual.DelayedCall(_enableDelay, tile.EnableDisabledGrass).SetId(this);
            else
                DOVirtual.DelayedCall(_enableDelay, tile.EnableGrass).SetId(this);
        }
    }
}