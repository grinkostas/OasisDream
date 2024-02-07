using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class TilePlacer : MonoBehaviour
    {
        [SerializeField] private float _tileWidth;
        [SerializeField] private float _tileHeight;

        [Button()]
        private void MoveLeft()
        {
            transform.position += Vector3.right * -_tileWidth;
        }

        [Button()]
        private void MoveRight()
        {
            transform.position += Vector3.right * _tileWidth;
        }

        [Button()]
        private void MoveUp()
        {
            transform.position += Vector3.forward * _tileHeight;
        }
        
        [Button()]
        private void MoveDown()
        {
            transform.position += Vector3.forward * -_tileHeight;
        }
    }
}