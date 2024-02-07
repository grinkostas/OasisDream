using System;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class IslandInstaller : MonoInstaller
    {
        [SerializeField] private Island _island;

        public override void InstallBindings()
        {
            Container.Bind<Island>().FromInstance(_island);
        }
    }
}