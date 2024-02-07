using System;
using JetBrains.Annotations;
using StaserSDK;
using UnityEngine;
using Zenject;


public class ForgePopup : APopup
{
    [Inject, UsedImplicitly] public InputHandler InputHandler { get; }
    
    public override Type Type => typeof(ForgePopup);

    protected override void OnShowStart()
    {
        InputHandler.DisableHandle(this);
    }

    protected override void OnHideStart()
    {
        InputHandler.EnableHandle(this);
    }
}

