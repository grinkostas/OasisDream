using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class TileRepresenter : MonoBehaviour
    {
        [SerializeField] private Transform _placedTileParent;
        [SerializeField] private Transform _spawnTilesParent;

        [Button()]
        private void Place()
        {
            List<Vector3> positions = new();
            foreach (Transform child in _placedTileParent)
            {
                if(child.gameObject.activeSelf == false)
                    continue;
                positions.Add(child.transform.position);
            }

            var tiles = _spawnTilesParent.gameObject.GetComponentsInChildren<Tile>();
            int count = Mathf.Min(tiles.Length, positions.Count);
            
            for (int i = 0; i < count; i++)
            {
                tiles[i].transform.position = positions[i];
            }
        }
    }
}