using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public class StackCountChangedFx : StackCountChangeListener
{
    [SerializeField] private Bouncer _bouncer;
   
    protected override void OnStackCountChanged(int count)
    {
        _bouncer.Bounce();
    }

}
