using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class AnimatorLinker : MonoBehaviour
{
    [SerializeField] private bool _needAnimatorLinker = false;
    [SerializeField, ShowIf(nameof(_needAnimatorLinker))] private AnimatorLinker _animatorLinker;
    [SerializeField, HideIf(nameof(_needAnimatorLinker))] private Animator _animator;

    public Animator Animator => _needAnimatorLinker ? _animatorLinker.Animator : _animator;

    public void SetBool(string parameterName, bool value)
    {
        _animator.SetBool(parameterName, value);
    }

    public void SetFloat(string parameterName, float value)
    {
        _animator.SetFloat(parameterName, value);
    }

    public void SetTrigger(string parameterName)
    {
        _animator.SetTrigger(parameterName);
    }
}
