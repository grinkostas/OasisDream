using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PlayerModifierInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ModifiersController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ModifiersSave>().AsSingle().NonLazy();
    }
}
