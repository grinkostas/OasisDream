using System;
using UnityEngine;

namespace GameCore.Scripts.Castle.Resources.ResourcePlaces
{
    public class FullHealthIInteractBlocker : MonoBehaviour
    {
        [SerializeField] private ResourcePlace _resourcePlace;
        [SerializeField] private Collider _interactCollider;

        private void OnEnable()
        {
            _resourcePlace.HealthChanged += Actualize;
            Actualize();
        }

        private void OnDisable()
        {
            _resourcePlace.HealthChanged -= Actualize;
        }

        private void Actualize()
        {
            bool active = _resourcePlace.Health >= _resourcePlace.MaxHealth;
            _resourcePlace.CheckActivityObject.SetActive(active);
            _interactCollider.enabled = active;
        }
    }
}