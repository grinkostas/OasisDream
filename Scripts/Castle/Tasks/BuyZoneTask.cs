using System.Linq;
using GameCore.Scripts.Tasks;
using JetBrains.Annotations;
using StaserSDK.Stack;
using UnityEngine;
using Zenject;


public class BuyZoneTask : ATask
{
    [SerializeField] private BuyZone _buyZone;
    
    private InitializedProperty<int> _currentValue;
    private InitializedProperty<float> _finishValue;

    protected override void OnInject()
    {
        _currentValue = new InitializedProperty<int>(() => _buyZone.UsedResources.Sum(x => x.Value));
        _finishValue = new InitializedProperty<float>(() => _buyZone.SourcePrices.Sum(x => x.Value));
        
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _buyZone.AbsoluteProgressChanged.On(OnUsedResource);
        _buyZone.Bought.Once(Finish);
        OnUsedResource(_currentValue.Value);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _buyZone.AbsoluteProgressChanged.Off(OnUsedResource);
        _buyZone.Bought.Off(Finish);
    }

    protected void OnUsedResource(int count)
    {
        _currentValue.Value = count;
        CurrentValueChanged.Dispatch(CurrentValue);
    }
    
    public override float GetCurrentProgress() => _buyZone.Progress;

    public override float GetFinishValue() =>_finishValue.Value;

    public override float GetCurrentValue() => _currentValue.Value;

    protected override bool IsFinishedInternal() => CurrentValue == FinishValue || _buyZone.IsBought();
    public override bool IsAvailableInternal() => _buyZone.gameObject.activeInHierarchy;
}
