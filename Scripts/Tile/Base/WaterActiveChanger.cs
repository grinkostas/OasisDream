using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class WaterActiveChanger : MonoBehaviour
    {
        [SerializeField] private Tile _tile;
        [SerializeField] private float _enableDelay;
        [SerializeField] private List<GameObject> _objectsToEnable;
        [SerializeField] private float _disableDelay;
        [SerializeField] private List<GameObject> _objectsToDisable;
        

        private void OnEnable()
        {
            if (_tile.IsWatered)
            {
                OnWatered();
                return;
            }
            
            SetActive(_objectsToDisable, true);
            SetActive(_objectsToEnable, false);
            
            _tile.Watered.Once(OnWatered);
        }

        private void OnDisable()
        {
            _tile.Watered.Off(OnWatered);
        }

        private void OnWatered()
        {
            DOTween.Kill(this);
            DOVirtual.DelayedCall(_enableDelay, () => SetActive(_objectsToEnable, true)).SetUpdate(false).SetId(this);
            DOVirtual.DelayedCall(_disableDelay, () => SetActive(_objectsToDisable, false)).SetUpdate(false).SetId(this);
        }

        private void SetActive(List<GameObject> objects, bool active)
        {
            foreach (var objectToChangeActive in objects)
            {
                objectToChangeActive.SetActive(active);
            }
        }
    }
}