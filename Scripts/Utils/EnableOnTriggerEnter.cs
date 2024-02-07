using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Scripts.Untils
{
    [RequireComponent(typeof(Collider))]
    public class EnableOnTriggerEnter : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _objectsToEnable;
        [SerializeField] private List<GameObject> _objectsToDisable;

        private bool _triggered = false;
        
        private void Awake()
        {
            _objectsToEnable.SetActive(false);
            _objectsToDisable.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            _triggered = true;
            OnTrigger();
        }

        public void Trigger()
        {
            if(_triggered)
                return;
            _triggered = true;
            OnTrigger();
        }

        private void OnTrigger()
        {
            _objectsToEnable.SetActive(true);
            _objectsToDisable.SetActive(false);
        }
    }
}