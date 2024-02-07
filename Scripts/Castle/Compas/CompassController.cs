 using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals;
using StaserSDK.Stack;
using Zenject;

public class CompassController : MonoBehaviour
{
    [SerializeField] private float _actualizeDelay;
    
    [Inject, UsedImplicitly] public BuyZoneController BuyZoneController { get; }
    [Inject, UsedImplicitly] public DiContainer Container { get; }
    [Inject, UsedImplicitly] public Player Player { get; }

    private List<CompassItemParser> _parsers = new();

    public Vector3 _currentTarget;
    public Vector3 CurrentTarget
    {
        get => _currentTarget;
        private set
        {
            if(_currentTarget == value)
                return;
            _currentTarget = value;
            NewTarget.Dispatch(_currentTarget);
        } 
    }
    public bool HaveTarget { get; private set; } = false;
    
    private bool _initialized = false;

    public TheSignal Hidden { get; } = new();
    public TheSignal<Vector3> NewTarget { get; } = new();


    private void Awake()
    {
        _parsers = new List<CompassItemParser>()
        {
            new BuyZoneParser().SetPriority(5), 
            new RecycleForBuyParser().SetPriority(3), 
            new RecyclersParser().SetPriority(0)
        };
        
        foreach (var parser in _parsers)
            Container.Inject(parser);

        _initialized = true;
    }

    private void OnEnable()
    {
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_actualizeDelay, Actualize).SetId(this);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }

    public void Actualize()
    {
        DOVirtual.DelayedCall(_actualizeDelay, Actualize).SetId(this);
        
        if(enabled == false || gameObject.activeInHierarchy == false)
            return;
        bool parsed = TryToParse();

        if (parsed == false)
            TryToHide();
        
    }

    private bool TryToParse()
    {
        var types = Player.Stack.MainStack.SourceItems.Select(x => x.Type).Distinct().ToList();
        foreach (var parser in _parsers)
        {
            var parserResult = parser.Parse(types);
            if (parserResult.Completed)
            {
                HaveTarget = true;
                CurrentTarget = parserResult.TargetPosition;
                return true;
            }
        }

        return false;
    }

    private bool TryToHide()
    {
        HaveTarget = false;
        CurrentTarget = Vector3.zero;
        Hidden.Dispatch();
        return true;
    }
}
