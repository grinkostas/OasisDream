using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageSound : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private List<AudioSource> _soundsQueue;

    private int _index = 0;
    
    private void OnEnable()
    {
        _destructible.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _destructible.Damaged -= OnDamaged;
    }

    private void OnDamaged(int damage)
    {
        if(_soundsQueue.Count == 0)
            return;
        if (_index >= _soundsQueue.Count)
            _index = 0;
        _soundsQueue[_index].Play();
        _index++;
    }
}
