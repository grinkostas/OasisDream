using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class CompassInstaller : MonoInstaller
{
    [SerializeField] private CompassController _compassController;
    
    public override void InstallBindings()
    {
        Container.Bind<CompassController>().FromInstance(_compassController);
    }
}
