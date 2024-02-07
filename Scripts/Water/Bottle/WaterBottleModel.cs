using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.Scripts.Water
{
    public class WaterBottleModel : MonoBehaviour
    {
        [SerializeField] private Transform _holder;
        [SerializeField] private Transform _waterGunWithHose;
        [SerializeField] private Transform _waterGun;
        [SerializeField] private Hose _hose;
        [SerializeField] private Transform _water;
        [SerializeField] private Transform _barrelWrapper;
        [SerializeField] private Transform _wrapper;

        public Transform Holder => _holder;
        public Transform WaterGunWithHose => _waterGunWithHose;
        public Transform WaterGun => _waterGun;
        public Hose Hose => _hose;
        public Transform Water => _water;
        public Transform BarrelWrapper => _barrelWrapper;
        public Transform Wrapper => _wrapper;
    }
}