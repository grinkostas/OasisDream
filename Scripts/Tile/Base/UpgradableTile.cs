using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.Scripts.Tiles
{
    public class UpgradableTile : MonoBehaviour
    {
        [SerializeField] private BuyZoneEvents _buyZone;
        [SerializeField] private Tile _tile;
        [SerializeField] private List<GameObject> _objectsToDisable;
        [SerializeField] private GameObject _activateObject;
        [Header("Rotation")] 
        [SerializeField] private Transform _rotateModel;
        [SerializeField] private Vector3 _startRotation;
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private float _rotationDuration;
        [SerializeField] private Ease _rotationEase;
        [Header("Zoom")] 
        [SerializeField] private float _zoomDuration; 
        [SerializeField] private Ease _zoonEase;
        [Header("Move")] 
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _moveDuration;
        

        private void OnEnable()
        {
            _rotateModel.rotation = Quaternion.Euler(_startRotation);
            if (_buyZone.IsBought())
            {
                Upgrade();
                return;
            }
            _activateObject.SetActive(false);
            _buyZone.Bought.Once(Upgrade);
        }

        private void Upgrade()
        {
            _activateObject.SetActive(true);
            if (_tile.IsWatered == false)
            {
                _tile.EnableGrass();
                foreach (var objectToDisable in _objectsToDisable)
                    objectToDisable.SetActive(false);
                _activateObject.transform.localScale = Vector3.zero;
                _activateObject.transform.DOScale(Vector3.one, _zoomDuration).SetEase(_zoonEase);
                _rotateModel.rotation = Quaternion.Euler(_rotation);
                return;
            }
            
            _tile.EnableGrass();
            _activateObject.transform.localScale = Vector3.one;
            _rotateModel.DOPunchPosition(Vector3.up * _jumpHeight, _moveDuration, 2);
            _rotateModel.DORotate(_rotation, _rotationDuration)
                .SetEase(_rotationEase);
        }

    }
}