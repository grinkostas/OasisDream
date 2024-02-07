using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.Scripts.Utilities
{
    public class ModelRandomizer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _models;
        [Header("Empty")]
        [SerializeField] private bool _randomEmpty;
        [SerializeField, Range(0, 100), ShowIf(nameof(_randomEmpty))] 
        private int _emptyChance;
        
        private void OnEnable()
        {
            foreach (var model in _models)
                model.SetActive(false);

            if (_randomEmpty)
            {
                int isEmptyRandomValue = Random.Range(0, 100);
                if(_emptyChance > isEmptyRandomValue)
                    return;
            }
            
            int random = Random.Range(0, _models.Count);
            _models[random].SetActive(true);
        }

        private void OnDisable()
        {
        }
    }
}