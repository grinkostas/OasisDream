using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using StaserSDK.Stack;
using StaserSDK.Utilities;
using Zenject;

public class CollectSound : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private List<STuple<List<ItemType>, List<AudioClip>>> _soundsData;
    [SerializeField] private AudioSource _audioSource;

    [Inject] private Player _player;

    private void OnEnable()
    {
        _player.Stack.MainStack.AddedItem += OnCountChanged;
    }
    
    private void OnDisable()
    {
        _player.Stack.MainStack.AddedItem -= OnCountChanged;
    }

    private void OnCountChanged(StackItemData data)
    {
        DOVirtual.DelayedCall(_delay, () => Play(data));
    }

    private void Play(StackItemData data)
    {
        var targetItemType = data.Target.Type;
        var targetTuple = _soundsData.Find(x => x.Value1.Has(x1 => x1 == targetItemType));
        if(targetTuple == null)
            return;
        var clip = targetTuple.Value2.Random();
        _audioSource.clip = clip;
        _audioSource.Play();

    }
}
