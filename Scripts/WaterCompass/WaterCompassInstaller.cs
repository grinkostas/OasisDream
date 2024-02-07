using UnityEngine;
using Zenject;

namespace GameCore.Scripts.WaterCompass
{
    public class WaterCompassInstaller : MonoInstaller
    {
        [SerializeField] private WaterCompass _waterCompass;

        public override void InstallBindings()
        {
            Container.Bind<WaterCompass>().FromInstance(_waterCompass);
        }
    }
}