using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Zenject;

public class WakeUpFx : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _particles;
    [SerializeField] private ModelHighlighter _modelHighlighter;
    [SerializeField] private HelperSleep _helperSleep;
    [SerializeField] private float _delayBetweenPlayerAttack;
    [SerializeField] private float _playerRotateDuration;
    [SerializeField] private float _attackDuration;

    [Inject, UsedImplicitly] public Player Player { get; }

    private const string PlayerAttackParameter = "AttackTrigger";
    private const string HelperHitParameter = "HitTrigger";

    private void OnEnable()
    {
        DOTween.Kill(this);
        _helperSleep.WokeUp.On(OnWokeUp);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
        _helperSleep.WokeUp.Off(OnWokeUp);
    }

    private void OnWokeUp()
    {
        DOTween.Kill(this);;
        TemporallyDisablePlayerHandle();
        StartCoroutine(LookAtHelper());
        CastMainEffect();
    }

    private void TemporallyDisablePlayerHandle()
    {
        Player.Movement.DisableHandle(this);
        DOVirtual.DelayedCall(_playerRotateDuration + _delayBetweenPlayerAttack +_attackDuration, () => Player.Movement.EnableHandle(this)).SetId(this);
    }
    
    private IEnumerator LookAtHelper()
    {
        float wastedTime = 0.0f;
        float duration = _playerRotateDuration + _delayBetweenPlayerAttack + _attackDuration;
        while (wastedTime <= duration)
        {
            wastedTime += Time.deltaTime;
            var relativePosition = _helperSleep.Helper.transform.position - Player.transform.position;
            var targetRotation = Vector3.Scale(Quaternion.Euler(relativePosition).eulerAngles, Vector3.up);
            float rotateProgress = Mathf.Min(1.0f, wastedTime / _playerRotateDuration);
            Quaternion.Lerp(Quaternion.Euler(targetRotation), Player.Model.rotation, rotateProgress);
            yield return null;
        }
    }

    private void CastMainEffect()
    {
        DOVirtual.DelayedCall(_playerRotateDuration, () => Player.Animator.SetTrigger(PlayerAttackParameter)).SetId(this);
        DOVirtual.DelayedCall(_playerRotateDuration + _delayBetweenPlayerAttack + _attackDuration, () =>
        {
            _helperSleep.Helper.Animator.SetTrigger(HelperHitParameter);
            _modelHighlighter.Highlight();
            PlayParticles();
        });

    }

    private void PlayParticles()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
            particle.time = 0;
            particle.Play();
        }
    }
    
    
}
