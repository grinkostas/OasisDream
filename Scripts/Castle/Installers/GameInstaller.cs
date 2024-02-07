using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameCore.Scripts.Tasks;
using StaserSDK.Utilities;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BuyZoneController _buyZoneController;
    [SerializeField] private SavesController _savesController;

    public override void InstallBindings()
    {
        Container.Bind<BuyZoneController>().FromInstance(_buyZoneController).AsSingle().NonLazy();
        Container.Bind<SavesController>().FromInstance(_savesController).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PauseManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RecyclersController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TaskController>().AsSingle().NonLazy();
    }
}
