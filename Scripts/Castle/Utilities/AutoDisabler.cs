using System;
using DG.Tweening;
using UnityEngine;


public class AutoDisabler : MonoBehaviour
{
    [SerializeField] private float _disableDelay;

    private void OnEnable()
    {
        DOTween.Kill(this);
        DOVirtual.DelayedCall(_disableDelay, () => gameObject.SetActive(false)).SetId(this);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }
}
