using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCore.Scripts.WaterCompass
{
    public class WaterCompass : MonoBehaviour
    {
        [SerializeField] private Transform _compassPointer;
        [SerializeField] private float _minDistance;

        private List<Transform> _targets = new();

        private void OnEnable()
        {
            if (_targets.Count == 0)
                ResetTarget();
        }

        public void AddTarget(Transform target)
        {
            if(_targets.Contains(target))
                return;
            _targets.Add(target);
        }

        public void RemoveTarget(Transform target)
        {
            _targets.Remove(target);
        }

        private void ResetTarget()
        {
            _compassPointer.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_targets.Count == 0 || 
                VectorExtentions.SqrDistance(_targets[0], transform) <= _minDistance * _minDistance)
            {
                ResetTarget();
                return;
            }

            LookAtTarget();
        }

        private void LookAtTarget()
        {
            _compassPointer.gameObject.SetActive(true);
            _compassPointer.LookAt(_targets[0]);
        }
    }
}