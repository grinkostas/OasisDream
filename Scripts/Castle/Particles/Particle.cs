using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public abstract class Particle : MonoBehaviour, IPoolItem<Particle>
{
    public IPool<Particle> Pool { get; set; }
    public bool IsTook { get; set; }
    
    public abstract Type Type { get; }
    
    private InitializedProperty<ParticleSystem> _particleSystemProperty;
    public ParticleSystem Instance
    {
         get
        {
            if (_particleSystemProperty == null)
                _particleSystemProperty =
                    new InitializedProperty<ParticleSystem>(() => gameObject.GetComponent<ParticleSystem>());
            return _particleSystemProperty.Value;
        }
    }

    public void Play()
    {
        Instance.Play();
    }

    public void Stop()
    {
        Instance.Stop();
    }

    public void Restart()
    {
        Instance.Stop();
        Instance.time = 0.0f;
        Instance.Play();
    }

    
    public void TakeItem()
    {
        Stop();
        Instance.time = 0.0f;
    }

    public void ReturnItem()
    {
        Stop();
        Instance.time = 0.0f;
    }
}
