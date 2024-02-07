using System;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class IslandFinisher : MonoBehaviour
    {
        [SerializeField] private BuyZone _buyZone;

        [Inject] public Island Island { get; }
        
        private void OnEnable()
        {
            _buyZone.Bought.On(OnBought);
        }

        private void OnDisable()
        {
            _buyZone.Bought.Off(OnBought);
        }

        private void OnBought()
        {
            Island.Finish();
        }
    }
}