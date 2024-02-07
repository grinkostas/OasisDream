using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Lean.Touch;
using NaughtyAttributes;
using StaserSDK;
using Zenject;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _defaultPov;
    [SerializeField] private float _zoomOutPov;
    [SerializeField] private float _zoomDuration;
    [Range(-1.0f, 1.0f)]
    [SerializeField] private float _wheelSensitivity;

    [Inject] private InputHandler _inputHandler;

    private float Fov
    {
        get => _virtualCamera.m_Lens.FieldOfView;
        set => _virtualCamera.m_Lens.FieldOfView = value;
    } 

    private void OnEnable()
    {
        _inputHandler.OnStartMove += ZoomIn;
    }

    private void OnDisable()
    {
        _inputHandler.OnStartMove -= ZoomIn;
    }

    private void Update()
    {
        var fingers = LeanTouch.GetFingers(false, false, 2);
        var pinchRatio =  LeanGesture.GetPinchRatio(fingers, _wheelSensitivity);
        if(pinchRatio < 1)
            ZoomIn();
        if(pinchRatio > 1)
            ZoomOut();
    }
    
    [Button]
    private void ZoomIn()
    {
        DOTween.Kill(this);
        DOVirtual.Float(Fov, _defaultPov, _zoomDuration, value => Fov = value).SetId(this);
    }
    
    [Button]
    private void ZoomOut()
    {
        DOTween.Kill(this);
        DOVirtual.Float(Fov, _zoomOutPov, _zoomDuration, value => Fov = value).SetId(this);
    }
}
