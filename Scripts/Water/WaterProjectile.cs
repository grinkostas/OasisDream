using UnityEngine;

namespace GameCore.Scripts.Water
{
    [RequireComponent(typeof(Collider))]
    public class WaterProjectile : MonoBehaviour
    {
        [SerializeField] private WaterBottle _waterBottle;

        public WaterBottle WaterBottle => _waterBottle;
    }
}