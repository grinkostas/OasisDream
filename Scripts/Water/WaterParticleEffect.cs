using UnityEngine;

namespace GameCore.Scripts.Water
{
    public class WaterParticleEffect : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _moveUpNormalizedDuration;
        [SerializeField] private float _distanceThreshold = 0.05f;
        [SerializeField] private float _upForce = 10.0f;
        [SerializeField] private ParticleSystem _particleSystem;
        
        private ParticleSystem.Particle[] particles;

        private bool _isPlaying = false;

        private bool _targetInitialized = false;
        private Transform _target;

        public void Play(Transform target)
        {
            if(_isPlaying)
                return;
            _target = target;
            _isPlaying = true;
            _targetInitialized = true;
            _particleSystem.Play();
        }

        public void Stop()
        {
            if(_isPlaying == false)
                return;
            _targetInitialized = false;
            _isPlaying = false;
            _particleSystem.Stop(true);
            
        }

        void LateUpdate()
        {
            particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
            int particleCount = _particleSystem.GetParticles(particles);

            int aliveParticles = 0;
            
            for (int i = 0; i < particleCount; i++)
            {
                float lifeTime = particles[i].startLifetime - particles[i].remainingLifetime;
                float normalizedLifeTime = lifeTime / particles[i].startLifetime;

                if (normalizedLifeTime < _moveUpNormalizedDuration)
                {
                    particles[i].velocity = new Vector3(0, _upForce, 0);
                    particles[aliveParticles] = particles[i];
                    aliveParticles++;
                    continue;
                }
                
                float lerpValue = _curve.Evaluate(normalizedLifeTime);
                particles[aliveParticles] = particles[i];
                particles[i].position = Vector3.Lerp(particles[i].position, _target.position, lerpValue);
                if (VectorExtentions.SqrDistance(particles[i].position, _target.position) >
                    _distanceThreshold * _distanceThreshold)
                {
                    particles[aliveParticles] = particles[i];
                    aliveParticles++;
                }
                    
                
            }

            GetComponent<ParticleSystem>().SetParticles(particles, aliveParticles);
        }
    }
}