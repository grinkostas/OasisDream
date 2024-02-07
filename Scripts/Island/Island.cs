using System.Collections.Generic;
using System.Linq;
using GameCore.Scripts.MarketLogic.Customers;
using GameCore.Scripts.Tiles;
using NepixSignals;
using UnityEngine;

namespace GameCore.Scripts
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private string _islandId;
        [SerializeField, Range(0, 1)] private float _completePercent = 0.9f;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Sprite _islandIcon;
        [SerializeField] private BuyZonesController _buyZonesController;
        [SerializeField] private bool _waterAllAtStart;
        
        private List<Requester> _requesters = new();
        public List<ResourcePlace> ResourcePlaces { get; private set; } = new();

        private List<Tile> _tiles = new List<Tile>();
        public List<Tile> Tiles => _tiles;

        public int MinTilesToComplete => Mathf.CeilToInt(TotalTilesCount * _completePercent);
        
        public Sprite Icon => _islandIcon;
        public BuyZonesController BuyZonesController => _buyZonesController;
        public int WateredTiles { get; private set; } = 0;
        public int TotalTilesCount => _tiles.Count;
        public Transform SpawnPoint => _spawnPoint;

        public bool IsCompleted => WateredTiles >= MinTilesToComplete;
        public bool IsFinished => ES3.Load(_islandId, false);
        
        public TheSignal WateredTile { get; } = new();
        public TheSignal AddedTile { get; } = new();
        public TheSignal Completed { get; } = new();
        public TheSignal Finished { get; } = new();
        
        
        public void AddRequester(Requester requester)
        {
            if(_requesters.Contains(requester))
                return;
            _requesters.Add(requester);
        }

        public void RemoveRequester(Requester requester)
        {
            _requesters.Remove(requester);
        }

        public Requester GetRequester()
        {
            return _requesters.OrderBy(x=> x.Queue.Count).FirstOrDefault();
        }
        
        public void AddPlace(ResourcePlace resourcePlace)
        {
            if(ResourcePlaces.Contains(resourcePlace))
                return;
            ResourcePlaces.Add(resourcePlace);
        }

        public void RemovePlace(ResourcePlace resourcePlace)
        {
            ResourcePlaces.Remove(resourcePlace);
        }

        public void AddTile(Tile tile)
        {
            _tiles.Add(tile);
            if(_waterAllAtStart)
                tile.EnableGrass();
            
            AddedTile.Dispatch();
            
            if (tile.IsWatered == false)
                tile.Watered.Once(OnWateredTile);
            else
                OnWateredTile();
        }

        private void OnWateredTile()
        {
            WateredTiles++;
            WateredTile.Dispatch();
            if(IsCompleted)
                Completed.Dispatch();
        }

        public void Finish()
        {
            ES3.Save(_islandId, true);
            Finished.Dispatch();
        }
        
    }
}