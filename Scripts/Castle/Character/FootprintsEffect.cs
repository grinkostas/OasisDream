using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK.Utilities;
using Zenject;

public class FootprintsEffect : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private int _poolSize;
    [SerializeField] private Sprite _footprint;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private List<Transform> _footPoints;
    [SerializeField] private Vector2 _spawnDistance;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _zoomTime;
    [SerializeField] private Color _color;
    
    
    private List<Footprint> _footprints = new List<Footprint>();

    private List<Footprint> Footprints
    {
        get
        {
            if (_footprints.Count == 0)
            {
                for (int i = 0; i < _poolSize; i++)
                {
                    _footprints.Add(new Footprint(_footprint, _startScale, _startRotation, _color));
                }
            }

            return _footprints;
        }
    }

    private int _currentFootprintIndex = 0;
    private int _currentLegIndex = 0;
    
    private Vector3 _previousPosition = Vector3.zero;
    private float _crossedDistance = 0.0f;
    private float _currentTargetDistance = 0.0f;

    private void OnEnable()
    {
        _currentTargetDistance = _spawnDistance.Random();
        _previousPosition = _followTarget.position;
    }

    private void Update()
    {
        if (_crossedDistance >= _currentTargetDistance)
        {
            ShowFootprint();
            _crossedDistance = 0.0f;
            _currentTargetDistance = _spawnDistance.Random();
            _previousPosition = _followTarget.position;
            return;
        }
        _crossedDistance += (_previousPosition.XZ() - _followTarget.position.XZ()).Abs().Sum();
        _previousPosition = _followTarget.transform.position;
    }

    private void ShowFootprint()
    {
        Footprints[_currentFootprintIndex].Show(_footPoints[_currentLegIndex], _lifeTime,_zoomTime);
        _currentFootprintIndex++;
        if (_currentFootprintIndex >= Footprints.Count)
            _currentFootprintIndex = 0;
        _currentLegIndex++;
        if (_currentLegIndex >= _footPoints.Count)
            _currentLegIndex = 0;

    }


    private class Footprint
    {
        private Vector3 _startScale;
        private Quaternion _startRotation;
        private SpriteRenderer _spriteRenderer;

        public Footprint(Sprite footprint, Vector3 startScale, Vector3 startRotation, Color color)
        {
            var footprintObject = new GameObject();
            _spriteRenderer = footprintObject.AddComponent<SpriteRenderer>();
            _spriteRenderer.sprite = footprint;
            _spriteRenderer.color = color;
            _startScale = startScale;
            _startRotation = Quaternion.Euler(startRotation);
            ResetTransform();
            footprintObject.SetActive(false);
        }

        private void ResetTransform()
        {
            var footprintObject = _spriteRenderer.gameObject;
            footprintObject.transform.localScale = _startScale;
            footprintObject.transform.rotation = _startRotation;
        }

        private void Hide(float zoomTime)
        {
            DOTween.Kill(this);
            _spriteRenderer.transform.DOScale(Vector3.zero, zoomTime)
                .OnComplete(()=> _spriteRenderer.gameObject.SetActive(false)).SetId(this);
        }
        
        public void Show(Transform followTransform, float lifeTime, float zoomTime)
        {
            
            DOTween.Kill(this);
            var transform = _spriteRenderer.transform;
            transform.position = followTransform.position;
            transform.localScale = Vector3.zero;
            transform.rotation = followTransform.rotation;
            _spriteRenderer.gameObject.SetActive(true);
            
            transform.DOScale(_startScale, zoomTime).SetEase(Ease.OutBack).SetId(this);
            DOVirtual.DelayedCall(lifeTime, () => Hide(zoomTime)).SetId(this);
        }
    }
}
