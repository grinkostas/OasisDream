using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class SkinManagerInstaller : MonoInstaller
{
    [SerializeField] private SkinManager _skinManager;

    public override void InstallBindings()
    {
        Container.Bind<SkinManager>().FromInstance(_skinManager);
    }
}
