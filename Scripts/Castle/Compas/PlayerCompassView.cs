using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class PlayerCompassView : MonoBehaviour
{
    [SerializeField] private Transform _compassParent;
    [SerializeField] private View _view;
    [SerializeField] private float _hideDistance;
    
    [Inject, UsedImplicitly] public CompassController CompassController { get; }

    private bool _viewHidden = true;
    
    private void OnEnable()
    {
        CompassController.NewTarget.On(OnNewTarget);
    }

    private void OnNewTarget(Vector3 target)
    {
        Show();
    }

    private void OnHide()
    {
        Hide();
    }

    private void Show()
    {
        if(_viewHidden)
            _view.Show();
        _viewHidden = false;
    }

    private void Hide()
    {
        if(_viewHidden == false)
            _view.Hide();
        _viewHidden = true;
    }
    
    private void Update()
    {
        if (CompassController.HaveTarget == false)
        {
            Hide();
            return;
        }

        _compassParent.LookAt(CompassController.CurrentTarget, Vector3.up);

        if (VectorExtentions.SqrDistance(_compassParent.position, CompassController.CurrentTarget) <
            _hideDistance * _hideDistance)
            Hide();
        else
            Show();
    }
}
