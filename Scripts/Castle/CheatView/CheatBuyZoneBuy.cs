using System.Collections.Generic;
using GameCore.Scripts.Castle.CheatView.Logic;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Castle.CheatView
{
    public class CheatBuyZoneBuy : CheatButtonBase
    {
        [SerializeField] private List<int> _buyZonesId;
        
        [Inject] public CheatContainer CheatContainer { get; }
        public override void OnButtonClicked()
        {
            foreach (var buyZoneId in _buyZonesId)
            {
                CheatContainer.StoredBuyZones[buyZoneId].ForceBuy();
            }
        }
    }
}