using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using GameCore.Scripts.UI.Misc;
using JetBrains.Annotations;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.Scripts.UI.Canvases
{
    public class WorldSpaceToCanvasSpaceConverter : MonoBehaviour
    {

        [Header("Follow")]
        [Tooltip("If followWorldObject is empty, than use worldPosition.")]
        [SerializeField] private Vector3 _worldPosition;
        [SerializeField] private Transform _followWorldObject;
        [SerializeField] private bool _smoothFollow;
        [SerializeField] private float _smoothFollowSpeed;
        [Space]
        public Vector2 offset;
        public Vector3 offsetWorld;
        [Space] 
        [SerializeField] private UpdateMethod _updateMethod;
        [SerializeField] private CanvasType _canvasType;

        [Inject, UsedImplicitly] public Camera camera { get; }
        [Inject, UsedImplicitly] public CanvasController CanvasController { get; }

        private List<object> _blockers = new();

        private Vector3 FollowPosition => (followWorldObject != null ? followWorldObject.position : worldPosition) + offsetWorld;
        public Canvas canvas => CanvasController.Get(_canvasType);
        
        public Vector3 worldPosition
        {
            get => _worldPosition;
            set
            {
                _worldPosition = value;
                Validate();
            }
        }

        public Transform followWorldObject
        {
            get => _followWorldObject;
            set
            {
                _followWorldObject = value;
                Validate();
            }
        }
        private RectTransform _rectTransformCached;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransformCached == null) _rectTransformCached = GetComponent<RectTransform>();
                return _rectTransformCached;
            }
        }

        private void OnEnable()
        {
            if (_updateMethod == UpdateMethod.None) return;
            
            Validate();
        }

        void Update()
        {
            if (_updateMethod == UpdateMethod.Update)
            {
                Validate(Time.deltaTime);
            }
        }
        
        void FixedUpdate()
        {
            if (_updateMethod == UpdateMethod.Fixed)
            {
                Validate(Time.fixedDeltaTime);
            }
        }
        
        void LateUpdate()
        {
            if (_updateMethod == UpdateMethod.Late)
            {
                Validate(Time.deltaTime);
            }
        }

        public void Validate(float dt = 0)
        {
            if(_blockers.Count > 0)
                return;
            
            if (canvas == null || camera == null) 
                return;
            
            Vector2 viewportPoint = camera.WorldToViewportPoint(FollowPosition);
            if (_smoothFollow)
            {
                var point = dt > 0 ? Vector2.Lerp(rectTransform.anchorMin, viewportPoint, dt * _smoothFollowSpeed) : viewportPoint;
                rectTransform.anchorMin = rectTransform.anchorMax = point;
            }
            else
            {
                rectTransform.anchorMin = rectTransform.anchorMax = viewportPoint;
            }
        }

        public void Enable(object sender)
        {
            _blockers.Remove(sender);
        }
        
        public void Disable(object sender)
        {
            if(_blockers.Contains(sender))
                return;
            _blockers.Add(sender);
        }

    }
}
