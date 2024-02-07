using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ReturnBaseState : HelperState
{
    [SerializeField] private float _refreshDelay;
    protected override void OnEnter()
    {
        if (Helper.Stack.ItemsCount == 0)
        {
            Exit();
            return;
        }

        Actualize();
        Helper.Stack.CountChanged += OnCountChanged;
        Helper.Handler.SetDestination(Helper.HelperHouse.ReturnPoint);
        
    }

    private void OnCountChanged(int count)
    {
        if (count == 0)
            Exit();
    }

    private void Actualize()
    {
        if(enabled == false)
            return;
        DOTween.Kill(this);
        if (Helper.Stack.ItemsCount == 0)
        {
            Exit();
            return;
        }
        DOVirtual.DelayedCall(_refreshDelay, Actualize).SetId(this);
    }

    protected override void OnExit()
    {
        DOTween.Kill(this);
        Helper.Stack.CountChanged -= OnCountChanged;
    }
}
