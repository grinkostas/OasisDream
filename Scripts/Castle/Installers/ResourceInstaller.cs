using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class ResourceInstaller : MonoInstaller
{
    [SerializeField] private ResourceController _resourceController;
    [SerializeField] private SoftCurrencyDisplay _softCurrencyDisplay;

    public override void InstallBindings()
    {
        Container.Bind<ResourceController>().FromInstance(_resourceController);
        Container.Bind<SoftCurrencyDisplay>().FromInstance(_softCurrencyDisplay);
    }
}
