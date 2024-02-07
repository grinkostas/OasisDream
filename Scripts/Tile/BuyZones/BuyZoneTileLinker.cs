using System;
using UnityEngine;

namespace GameCore.Scripts.Tiles.BuyZones
{
    public class BuyZoneTileLinker : MonoBehaviour
    {
        [SerializeField] private Tile _tileToLink;
        [SerializeField] private Transform _buyZoneWrapper;

        private void OnEnable()
        {
            _buyZoneWrapper.gameObject.SetActive(false);
            if (_tileToLink.IsWatered == false)
                _tileToLink.Watered.Once(Activate);
            else 
                Activate();
        }

        private void Activate()
        {
            _buyZoneWrapper.gameObject.SetActive(true);
        }
    }
}