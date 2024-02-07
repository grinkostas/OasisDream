using UnityEngine;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;

public class DestructibleRespawner : MonoBehaviour
{
    [SerializeField] private Destructible _destructible;
    [SerializeField] private float _respawnTime;
    [SerializeField] private float _additionalTimePerAttack;
    [SerializeField] private float _startRegenerationDelay = 3.0f;

    private float _waitedTime;
    
    public FloatModifier RespawnModifier { get; set; } = new FloatModifier(1, NumericAction.Multiply);
    
    private void OnEnable()
    {
        _destructible.Damaged += OnHealthChanged;
    }

    private void OnHealthChanged(int damage)
    {
        if(_destructible.Health == _destructible.MaxHealth)
            return;

        DOTween.Kill(this);
        DOVirtual.DelayedCall(_startRegenerationDelay, StartRespawn).SetId(this);
    }

    private void StartRespawn()
    {
        DOTween.Kill(this);
        float respawnTime = _respawnTime + _destructible.Damage * _additionalTimePerAttack;
        respawnTime = RespawnModifier.ApplyModifier(respawnTime);
        DOVirtual.DelayedCall(respawnTime, Respawn).SetId(this);
    }

    [Button()]
    private void Respawn()
    {
        DOTween.Kill(this);
        if(gameObject.activeInHierarchy)
            _destructible.Respawn();
    }

}
