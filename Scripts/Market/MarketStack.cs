using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using StaserSDK.Stack;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.Scripts.MarketLogic
{
    public class MarketStack : MonoBehaviour
    {
        [SerializeField] private List<Transform> _stakcItemPoints;
        [SerializeField] private Quaternion _rotation;
        [SerializeField] private float _zoomTime;
        
        [Inject, UsedImplicitly] public Market Market{ get; }
        [Inject, UsedImplicitly] public ResourceController ResourceController { get; }

        private List<StackItem> _representItems = new();

        [Inject]
        public void Inject()
        {
            for (int i = 0; i < _stakcItemPoints.Count; i++)
            {
                var item = ResourceController.GetInstance(Market.SellType);
                InitItem(item, i);
            }
        }

        private void InitItem(StackItem item, int index)
        {
            Transform itemTransform = item.transform;
            item.Claim();
            itemTransform.SetParent(_stakcItemPoints[index]);
            itemTransform.localPosition = Vector3.zero;
            itemTransform.rotation = _rotation;
            itemTransform.localScale = Vector3.zero;
            _representItems.Add(item);
        }

        private void OnEnable()
        {
            Actualize();
        }

        public Vector3 GetDelta()
        {
            if(_stakcItemPoints.Count == 0)
                return Vector3.zero;
            
            return _stakcItemPoints[GetCellIndex()].localPosition;
        }

        private int GetCellIndex()
        {
            int oneCellCount = Market.Capacity / _stakcItemPoints.Count;
            int cellIndex = Market.StoredAmount / oneCellCount;
            cellIndex = Math.Min(cellIndex, _stakcItemPoints.Count-1);
            return cellIndex;
        }
        
        public void Actualize()
        {
            int usedCells = GetCellIndex();
            DOTween.Kill(this);
            for (int i = 0; i < _stakcItemPoints.Count; i++)
            {
                Transform itemTransform = _representItems[i].transform;
                if(usedCells > i)
                    itemTransform.DOScale(Vector3.one, _zoomTime).SetEase(Ease.OutBack).SetId(this);
                else
                    itemTransform.DOScale(0, _zoomTime).SetEase(Ease.InBack).SetId(this);
            }
        }
    }
}