using System;
using UnityEngine;

namespace GameCore.Scripts.Utilities
{
    public class RotationFreezer : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private bool _local;
        private void LateUpdate()
        {
            if (_local)
                transform.localRotation = Quaternion.Euler(_rotation);
            else
                transform.rotation = Quaternion.Euler(_rotation);
        }
    }
}