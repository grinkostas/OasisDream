using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Scripts.Tiles;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK.Upgrades;
using UnityEngine;

namespace GameCore.Scripts.Water
{
    public class WaterBottle : MonoBehaviour
    {
        [SerializeField] private WaterBottleModel _waterBottleModel;
        [SerializeField, HideIf(nameof(_infinite))] private UpgradeValue _capacityValue;
        [SerializeField] private Collider _collider;
        [SerializeField] private WeaponController _weaponController;
        [SerializeField] private bool _infinite;
        [SerializeField] private float _startWateringDelay;
        [SerializeField] private float _nextTileWateringDelay;
        [SerializeField] private float _stopWateringDelay;
        public int MaxCapacity => _infinite ? int.MaxValue : _capacityValue.ValueInt;

        private int _capacity = 0;
        private int Capacity => _infinite ? MaxCapacity : _capacity;

        private List<Tile> _tilesInQueue = new();

        private bool _isWateringStarted = false;

        public WaterBottleModel WaterBottleModel => _waterBottleModel;
        public bool IsFull => WaterAmount == MaxCapacity;

        public int WaterAmount
        {
            get => Capacity;
            set
            {
                value = Mathf.Clamp(value, 0, MaxCapacity);
                if(value != _capacity)
                    WaterAmountChanged.Dispatch(value);
                _capacity = value;
            }
        }

        public UpgradeValue CapacityUpgradeValue => _capacityValue;

        public TheSignal<int> WaterAmountChanged { get; } = new();
        public TheSignal AddedWater { get; } = new();
        public TheSignal UsedWater { get; } = new();

        public TheSignal StartedUsage { get; } = new();
        public TheSignal EndedUsage { get; } = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Tile tile))
            {
                if (tile.IsWatered == false)
                    _tilesInQueue.Add(tile);
                Actualize();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Tile tile))
            {
                _tilesInQueue.Remove(tile);
                Actualize();
            }
        }

        private void Actualize()
        {
            List<Tile> tempTiles = new(_tilesInQueue);
            foreach (var tile in tempTiles)
            {
                if (tile.IsWatered)
                    _tilesInQueue.Remove(tile);
            }

            if (_tilesInQueue.Count == 0)
                StopWatering();
            else
                StartWatering();
        }
        
        public void AddWater(int newWaterAmount)
        { 
            if(WaterAmount >= newWaterAmount)
                return;
            WaterAmount = newWaterAmount;
            AddedWater.Dispatch();
        }
        
        private void StartWatering()
        {
            if(DOTween.IsTweening(_startWateringDelay)|| _isWateringStarted || WaterAmount <= 0)
                return;
            
            _weaponController.Disable(this);
            
            StartedUsage.Dispatch();
            DOTween.Kill(_stopWateringDelay);
            DOVirtual.DelayedCall(_startWateringDelay, () =>
            {
                _isWateringStarted = true;
                Watering();
            }).SetId(_startWateringDelay);
        }

        private void StopWatering()
        {
            void EndUsage()
            {
                if (_isWateringStarted == false) 
                    return;
                DOTween.Kill(_startWateringDelay);
                DOTween.Kill(_nextTileWateringDelay);
                EndedUsage.Dispatch();
                _isWateringStarted = false;
                _weaponController.Enable(this);
            }

            if (WaterAmount == 0)
            {
                EndUsage();
                return;
            }
            
            if(DOTween.IsTweening(_stopWateringDelay))
                return;
            
            DOVirtual.DelayedCall(_stopWateringDelay, EndUsage).SetId(_stopWateringDelay);
        }

        private void Watering()
        {
            var nearestTile = GetNearestTile();
            DOVirtual.DelayedCall(_nextTileWateringDelay, Watering).SetId(_nextTileWateringDelay);
            if (nearestTile == default || WaterAmount <= 0)
            {
                return;
            }
            DOTween.Kill(_stopWateringDelay);
            UseWater();
            nearestTile.EnableGrass();
            _tilesInQueue.Remove(nearestTile);
            if(_tilesInQueue.Count == 0)
                StopWatering();
        }
        
        private Tile GetNearestTile()
        {
            if (_tilesInQueue.Count == 0 )
                return null;
            return _tilesInQueue.OrderBy(tile => VectorExtentions.SqrDistance(tile.transform, transform))
                .FirstOrDefault();
        }

        [Button()]
        public void AddWater()
        {
            WaterAmount += 1;
            AddedWater.Dispatch();
            StopWatering();
        }

        public void UseWater()
        {
            if(_infinite)
                return;
            WaterAmount--;
            UsedWater.Dispatch();
            if(WaterAmount <= 0)
                StopWatering();
        }

        public void MakeInfinite()
        {
            _infinite = true;
        }
        
    }
}