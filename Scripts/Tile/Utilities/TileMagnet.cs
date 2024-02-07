using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class TileMagnet : MonoBehaviour
    {
        [SerializeField] private Vector3 _additionalPosition = Vector3.zero;
        [SerializeField] private Tile _selfTile;
            
        [Button()]
        public void Magnet()
        {
            var tiles = FindObjectsOfType<Tile>(true).ToList();
            if (_selfTile != null)
                tiles.Remove(_selfTile);
            var tile = tiles.OrderBy(x => VectorExtentions.SqrDistance(transform, x.transform)).FirstOrDefault();
            if(tile == default)
                return;
            transform.position = tile.transform.position + _additionalPosition;
        }
    }
}