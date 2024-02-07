using UnityEngine;
using Zenject;

namespace GameCore.Scripts.BuyZones
{
    public class ZonesControllerInstaller : MonoInstaller
    {
        [SerializeField] private BuyZonesController _buyZonesController;

        public override void InstallBindings()
        {
            Container.Bind<BuyZonesController>().FromInstance(_buyZonesController).AsSingle().NonLazy();
        }
    }
}