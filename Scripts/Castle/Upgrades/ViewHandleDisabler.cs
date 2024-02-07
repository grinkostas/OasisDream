using System;
using JetBrains.Annotations;
using StaserSDK;
using UnityEngine;
using Zenject;


public class ViewHandleDisabler : MonoBehaviour
{
    [SerializeField] private View _view;

    [Inject, UsedImplicitly] public InputHandler InputHandler { get; }
    private void OnEnable()
    {
        _view.ShowStart.On(OnShow);
        _view.HideStart.On(OnHide);
    }

    private void OnDisable()
    {
        _view.ShowStart.Off(OnShow);
        _view.HideStart.Off(OnHide);
    }

    private void OnShow()
    {
        InputHandler.DisableHandle(this);
        InputHandler.gameObject.SetActive(false);
    }

    private void OnHide()
    {
        InputHandler.EnableHandle(this);
        InputHandler.gameObject.SetActive(true);
    }
}
