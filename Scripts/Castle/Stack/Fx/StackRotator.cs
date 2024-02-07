using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Stack;

public class StackRotator : MonoBehaviour
{
    [Header("Rotate")]
    [SerializeField] private Vector3 _rotateAxis;
    [SerializeField] private float _rotateSpeed;
    
    public void Rotate(StackItem item, float duration)
    {
        var routine = StartCoroutine(Rotating(item, duration));
    }

    private IEnumerator Rotating(StackItem stackItem, float duration)
    {
        float wastedTime = 0.0f;
        while (wastedTime <= duration)
        {
            stackItem.Wrapper.localRotation *= Quaternion.Euler(_rotateAxis * (_rotateSpeed * Time.deltaTime));
            wastedTime += Time.deltaTime;
            yield return null;
        }

        stackItem.Wrapper.localRotation = Quaternion.identity;

    }
}
