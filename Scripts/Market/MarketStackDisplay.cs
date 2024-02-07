using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MarketLogic
{
    public class MarketStackDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private Bouncer _bouncer;
        [Inject, UsedImplicitly] public Market Market{ get; }
        
        private void OnEnable()
        {
            Actualize();
            Market.Added.On(Actualize);
            Market.Sold.On(Actualize);
        }

        private void OnDisable()
        {
            Market.Added.Off(Actualize);
            Market.Sold.Off(Actualize);
        }

        private void Actualize()
        {
            _bouncer.Bounce();
            _amountText.text = $"{Market.StoredAmount}/{Market.Capacity}";
        }
    }
}