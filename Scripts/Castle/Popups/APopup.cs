using System.Linq;
using DG.Tweening;
using GameCore.Scripts.Popups;
using NepixSignals;
using UnityEngine;
using Zenject;

public abstract class APopup : MonoBehaviour
{
    [SerializeField] private CanvasType _canvasType;
    [SerializeField] private Transform _contentModel;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _zoom = true;
    [SerializeField] private bool _fade = true;

    public CanvasType CanvasType => _canvasType;
    
    public Transform ContentModel => _contentModel;
    public PopupSettings Settings => PopupSettings.Value;
    
    public abstract System.Type Type { get; }
    public bool IsHidden { get; private set; } = true;
    
#region Signals
    public TheSignal ShowStarted { get; } = new();
    public TheSignal ShowCompleted { get; } = new();
    
    public TheSignal HideStarted { get; } = new();
    public TheSignal HideCompleted{ get; } = new();
    #endregion

#region Virtual Methods
    protected virtual void OnShowStart(){}
    protected virtual void OnShowComplete() { }
    
    protected virtual void OnHideStart(){}
    protected virtual void OnHideComplete() { }
    
#endregion

#region Show
    public void Show() => Show(0);
    public void Show(float delay)
    {
        if(IsHidden == false)
            return;
        
        ShowPrepare();
        Animate(delay, 1, Settings.In);
        CompleteShow(delay);
    }

    private void ShowPrepare()
    {
        IsHidden = false;
        DOTween.Kill(this);
        _contentModel.transform.localScale = Vector3.zero;
        _canvasGroup.alpha = 0.0f;
        OnShowStart();
        ShowStarted.Dispatch();
    }

    private void CompleteShow(float delay)
    {
        DOVirtual.DelayedCall(delay + Settings.In.Duration, () =>
        {
            IsHidden = false;
            OnShowComplete();
            ShowCompleted.Dispatch();
        }).SetId(this);
    }
#endregion

#region Hide
    public void Hide() => Hide(0.0f);

    public void Hide(float delay)
    {
        if(IsHidden)
            return;
        HidePrepare();

        Animate(delay, 0, Settings.Out);
        CompleteHide(delay);
    }
    private void HidePrepare()
    {
        IsHidden = true;
        DOTween.Kill(this);
        OnHideStart(); 
        HideStarted.Dispatch();
    }

    private void CompleteHide(float delay)
    {
        DOVirtual.DelayedCall(delay + Settings.Out.Duration, () =>
        {
            IsHidden = true;
            OnHideComplete();
            HideCompleted.Dispatch();
        }).SetId(this);
    }
#endregion

    public void Inject(DiContainer container)
    {
        container.BindInstance(this);
    }
    private void Animate(float delay, float endValue, PopupAnimationData animationData)
    {
        if(_zoom)
            _contentModel.DOScale(endValue,animationData.Duration)
            .SetEase(animationData.ZoomEase)
            .SetId(this).SetDelay(delay);
        if(_fade)
            _canvasGroup.DOFade(endValue, animationData.Duration)
            .SetEase(animationData.FadeEase)
            .SetId(this).SetDelay(delay);
    }
   
}
