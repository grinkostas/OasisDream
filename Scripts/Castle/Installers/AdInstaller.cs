using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class AdInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Interstitials>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Rewards>().AsSingle().NonLazy();
    }
}
