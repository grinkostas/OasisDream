using System;
using DG.Tweening;
using GameCore.Scripts.Water;
using NaughtyAttributes;
using NepixSignals;
using StaserSDK.SaveProperties.Api;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject _wateredModel;
        [SerializeField] private GameObject _wateredWithoutGrassModel;
        [SerializeField] private bool _wateredOnStart;

        [Inject] public Island Island { get; }

        private bool _isWatered
        {
            get
            {
                if (_isWateredInitialized == false && SavesController.EnableSaves)
                {
                    _isWateredInitialized = true;
                    _isWateredCached = ES3.Load(SaveString, false);
                }

                return _isWateredCached;
            }
        }

        private bool _isWateredInitialized = false;
        private bool _isWateredCached = false;
        public bool IsWatered => _wateredOnStart || _isWatered;
        
        private WaterBottle _waterBottle;
        
        private string SaveString => Vector3.Scale(transform.position, new Vector3(1, 0, 1)).ToString();

        public TheSignal Watered { get; } = new();

        [Inject]
        public void OnInject()
        {
            if(Island == null)
                return;
            Island.AddTile(this);
        }
        
        private void Awake()
        {
            if (_wateredOnStart)
                SetIsWatered();
            if (IsWatered == false)
            {
                _wateredModel.SetActive(false);
                _wateredWithoutGrassModel.SetActive(false);
            }
        }
        
        [Button()]
        public void EnableGrass()
        {
            if(_isWatered)
                return;
            SetIsWatered();
            _wateredModel.SetActive(true);
            Watered.Dispatch();
        }

        private void SetIsWatered()
        {
            _isWateredCached = true;
            if(SavesController.EnableSaves == false)
                return;
            ES3.Save(SaveString, true);
        }

        [Button()]
        public void EnableDisabledGrass()
        {
            EnableGrass();
            _wateredModel.transform.DOScaleY(0.5f, 0.35f).SetDelay(0.35f);
            

        }
    }
}