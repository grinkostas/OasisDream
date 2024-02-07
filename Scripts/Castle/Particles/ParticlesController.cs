using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] private Transform _particlesParent;
    [SerializeField] private List<Particle> _particlePrefabs;
    [SerializeField] private int _poolSize;

    private List<SimplePool<Particle>> _pools;

    public List<SimplePool<Particle>> Pools
    {
        get
        {
            if (_pools == null)
            {
                _pools = new();
                foreach (var particlePrefab in _particlePrefabs)
                {
                    var pool = new SimplePool<Particle>(particlePrefab, _poolSize, _particlesParent);
                    pool.Initialize();
                    _pools.Add(pool);
                }
            }
            return _pools;
        }
    }

    public Particle Get<TParticle>() where TParticle : Particle
    {
        var targetPool = Pools.Find(x => x.Prefab.Type == typeof(TParticle));
        if (targetPool == null)
            return null;
        return targetPool.Get();
    }

    public Particle Create<TParticle>(Vector3 position, float returnDelay) where TParticle : Particle
    {
        var particle = Get<TParticle>();
        particle.transform.position = position;
        particle.Restart();
        DOVirtual.DelayedCall(returnDelay, () => particle.Pool.Return(particle));
        return particle;
    }

}
