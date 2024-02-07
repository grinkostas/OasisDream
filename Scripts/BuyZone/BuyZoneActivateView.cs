using System;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class BuyZoneActivateView : MonoBehaviour
    {
        [SerializeField] private GameObject _view;
        [SerializeField] private bool _linker;
        [SerializeField, HideIf(nameof(_linker))] private BuyZone _buyZone;
        [SerializeField, ShowIf(nameof(_linker))] private BuyZoneLinker _buyZoneLinker;
        [Inject] public Island Island { get; }

        private BuyZone BuyZone => _linker ? _buyZoneLinker.Zone : _buyZone;
        
        private BuyZonesController Controller => Island.BuyZonesController;

        private void Awake()
        {
            _view.SetActive(false);
            if (Controller.IsZoneAvailable(BuyZone) == false)
                Controller.ActivatedZone.On(OnActivatedZone);
            else
                Activate();
        }

        private void Activate()
        {
            _view.SetActive(true);
        }

        private void OnActivatedZone(ABuyZoneActivator activator)
        {
            if(activator.Has(BuyZone))
                Activate();
        }
    }
}