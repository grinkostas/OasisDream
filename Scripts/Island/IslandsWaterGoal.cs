using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GameCore.Scripts
{
    public class IslandsWaterGoal : MonoBehaviour
    {
        [SerializeField] private IslandsController _islandsController;
        [SerializeField] private Image _islandIcon;
        [SerializeField] private SimpleSlider _progressSlider;

        private Island _currentIsland;
        private void Start()
        {
            NextIsland();
        }

        private void NextIsland()
        {
            var targetIsland =
                _islandsController.Islands.Find(x => x.gameObject.activeInHierarchy == true && x.IsFinished == false);
            if(targetIsland == _currentIsland)
                return;
            SetIsland(targetIsland);
        }

        private void SetIsland(Island island)
        {
            if (_currentIsland != null)
            {
                _currentIsland.AddedTile.Off(Actualize);
                _currentIsland.WateredTile.Off(Actualize);
                _currentIsland.Finished.Off(NextIsland);
            }
            _currentIsland = island;
            _islandIcon.sprite = _currentIsland.Icon;
            _currentIsland.AddedTile.On(Actualize);
            _currentIsland.WateredTile.On(Actualize);
            _currentIsland.Finished.On(NextIsland);
            Actualize();
        }

        private void Actualize()
        {
            int wateredTiles = _currentIsland.WateredTiles;
            int tilesCount = _currentIsland.MinTilesToComplete;
            _progressSlider.Value =  wateredTiles/ (float)tilesCount;
            if(wateredTiles >= tilesCount)
                NextIsland();
        }
    }
}