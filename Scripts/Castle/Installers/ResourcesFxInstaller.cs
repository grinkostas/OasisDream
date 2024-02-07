using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Scripts.Stack;
using Zenject;

public class ResourcesFxInstaller : MonoInstaller
{
    [SerializeField] private ResourcesFx _resourcesFx;
    [SerializeField] private ResourceCanvasFx _resourcesCanvasFxFx;

    public override void InstallBindings()
    {
        Container.Bind<ResourcesFx>().FromInstance(_resourcesFx);
        Container.Bind<ResourceCanvasFx>().FromInstance(_resourcesCanvasFxFx);
    }
}
