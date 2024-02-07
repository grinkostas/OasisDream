using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

namespace GameCore.Scripts.Tiles
{
    public class TilesRotator : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private Vector3 _rotateAxis;
        [SerializeField] private List<float> _possibleAngles;

        [Button()]
        private void Rotate()
        {
            var tiles = _parent.GetComponentsInChildren<Tile>();
            foreach (var tile in tiles)
                tile.transform.rotation = Quaternion.Euler(_possibleAngles.Random() * _rotateAxis);
        }

    }
}