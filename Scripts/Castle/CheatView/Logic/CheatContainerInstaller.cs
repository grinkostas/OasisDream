using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Castle.CheatView.Logic
{
    public class CheatContainerInstaller : MonoInstaller
    {
        [SerializeField] private CheatContainer _cheatContainer;

        public override void InstallBindings()
        {
            Container.Bind<CheatContainer>().FromInstance(_cheatContainer);
        }
    }
}