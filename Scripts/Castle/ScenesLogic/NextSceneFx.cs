using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using JetBrains.Annotations;
using StaserSDK;
using UnityEngine;
using Zenject;


public class NextSceneFx : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _endSceneCamera;
    
    [SerializeField] private float _uiShiwDelay;
    [SerializeField] private View _uiView;
    [SerializeField] private List<CanvasGroup> _fadeOutUi;

    [Inject, UsedImplicitly] public Player Player { get; }
    
    public void Show()
    {
        foreach (var group in _fadeOutUi)
        {
            group.DOFade(0, 0.5f);
            DOVirtual.DelayedCall(0.55f, () => group.gameObject.SetActive(false));
        }
        _endSceneCamera.gameObject.SetActive(true);
        
        DOVirtual.DelayedCall(_uiShiwDelay, _uiView.Show);
        Player.Movement.DisableHandle(this);
    }
}
