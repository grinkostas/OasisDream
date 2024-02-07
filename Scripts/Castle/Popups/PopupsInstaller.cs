using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Popups
{
    public class PopupsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CanvasController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PopupFactory>().AsSingle().NonLazy();
        }
    }
}