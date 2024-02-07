using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class BoostersControllerInstaller : MonoInstaller
{
    [SerializeField] private BoostersController _boostersController;

    public override void InstallBindings()
    {
        Container.Bind<BoostersController>().FromInstance(_boostersController);
    }
}
