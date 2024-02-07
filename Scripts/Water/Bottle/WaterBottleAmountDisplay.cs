using NaughtyAttributes;
using StaserSDK.Stack;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Water
{
    public class WaterBottleAmountDisplay : MonoBehaviour
    {
        [SerializeField] private SimpleSlider _slider;
        [SerializeField] private WaterBottle _waterBottle;

        private void OnEnable()
        {
            _waterBottle.WaterAmountChanged.On(Actualize);
        }
    
        private void OnDisable()
        {
            _waterBottle.WaterAmountChanged.Off(Actualize);
        }

        private void Start()
        {
            Actualize(_waterBottle.WaterAmount);
        }

        private void Actualize(int amount)
        {
            _slider.Value = (float)amount / _waterBottle.MaxCapacity;
        }
    }
}