using System;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.WaterCompass
{
    public class WaterCompassTarget : MonoBehaviour
    {
        [SerializeField] private BuyZoneLinker _buyZoneLinker;
        [Inject] public WaterCompass WaterCompass { get; }
        [Inject] public Island Island { get; }

        private void OnEnable()
        {
            if(_buyZoneLinker.Zone.IsBought())
                return;
            
            WaterCompass.AddTarget(transform);
            _buyZoneLinker.Zone.Bought.Once(Remove);
            Island.Finished.Once(Remove);


        }

        private void OnDisable()
        {
            Remove();
            _buyZoneLinker.Zone.Bought.Off(Remove);
        }

        private void Remove()
        {
            WaterCompass.RemoveTarget(transform);
        }
    }
}