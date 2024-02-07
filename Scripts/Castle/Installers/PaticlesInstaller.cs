using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PaticlesInstaller : MonoInstaller
{
    [SerializeField] private ParticlesController _particlesController;

    public override void InstallBindings()
    {
        Container.Bind<ParticlesController>().FromInstance(_particlesController);
    }
}
