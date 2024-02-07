using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MarketLogic
{
    public class MarketInstaller : MonoInstaller
    {
        [SerializeField] private Market _market;

        public override void InstallBindings()
        {
            Container.Bind<Market>().FromInstance(_market);
        }
    }
}