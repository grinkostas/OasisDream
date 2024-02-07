using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class IslandsControllerInstaller : MonoInstaller
    {
        [SerializeField] private IslandsController _islandsController;

        public override void InstallBindings()
        {
            Container.Bind<IslandsController>().FromInstance(_islandsController);
        }
    }
}