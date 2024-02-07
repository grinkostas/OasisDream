using System;
using System.Collections.Generic;
using GameCore.Scripts.Tiles;
using UnityEngine;

namespace GameCore.Scripts.Water
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class WateredZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Tile tile))
            {
                tile.EnableGrass();
            }
        }
    }
}