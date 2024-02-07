using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WaitHelperState : HelperState
{
    [SerializeField] private float _waitTime;

    public bool CanExit => HouseStack.ItemsCount < HouseStack.MaxSize;
    private StackBase HouseStack => Helper.HelperHouse.Stack;
    protected override void OnEnter()
    {
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_waitTime, TryToExit).SetId(this);
    }
    
    private void TryToExit()
    {
        if (CanExit)
        {
            Exit();
            return;
        }

        DOTween.Kill(this);
        DOVirtual.DelayedCall(_waitTime, TryToExit).SetId(this);
    }

    public void KillTween()
    {
        DOTween.Kill(this);
    }
    

    protected override void OnExit()
    {
        DOTween.Kill(this);
    }
}
