using System;
using System.Collections.Generic;
using UnityEngine;


public class ChestOpenFx : MonoBehaviour
{
    [SerializeField] private Animator _chestAnimator;
    [SerializeField] private List<Transform> _particleObjects;
    [SerializeField] private TweenAnimation<Transform> _hideParticlesAniamtion;
    [SerializeField] private EnterZoneRewardActivator _enterZoneRewardActivator;
    
    private void OnEnable()
    {
        _enterZoneRewardActivator.Activated.Once(OnActivated);
    }

    private void OnDisable()
    {
        _enterZoneRewardActivator.Activated.Off(OnActivated);
    }

    private void OnActivated()
    {
        _chestAnimator.enabled = true;
        foreach (var particleObject in _particleObjects)
        {
            _hideParticlesAniamtion.Animate(particleObject);
        }
    }
}
