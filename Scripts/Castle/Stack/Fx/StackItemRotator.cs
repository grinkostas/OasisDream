using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using StaserSDK.Stack;
using Unity.VisualScripting;

public class StackItemRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _startRotateAxis;
    [SerializeField] private bool _differentEndRotateAxis;
    [SerializeField, ShowIf(nameof(_differentEndRotateAxis))] private Vector3 _endRotateAxis;
    [SerializeField] private Vector2 _rotateSpeedRange;
    [SerializeField] private float _completeRotateDuration;
    
    public void Rotate(Transform target, float moveDuration)
    {
        StartCoroutine(Rotating(target, moveDuration));
    }

    public void Rotate(Transform target, float moveDuration, Vector3 destinationRotation, bool localRotate = false)
    {
        StartCoroutine(Rotating(target, moveDuration, true, destinationRotation, localRotate));
    }

    public void Rotate(Transform target, float moveDuration, Transform targetParent)
    {
        StartCoroutine(Rotating(target, moveDuration, true, Vector3.zero, true));
        DOVirtual.DelayedCall(moveDuration - _completeRotateDuration,
            () => StartCoroutine(LocalRotate(target, targetParent)));
    }
    
    private IEnumerator Rotating(Transform target, float moveDuration, bool haveDestination = false, Vector3 destinationRotation = new Vector3(), bool localRotate = false)
    {
        float wastedTime = 0.0f;
        float endRotateTime = 0.0f;
        var vectorSeeds = new Vector3(RandomSpeed(), RandomSpeed(), RandomSpeed());
        

        if (haveDestination)
        {
            moveDuration -= _completeRotateDuration;
            if(localRotate == false)
                target.DORotate(destinationRotation, _completeRotateDuration).SetDelay(moveDuration);
        }

        while (wastedTime < moveDuration)
        {
            Vector3 frameDelta = Vector3.Scale(vectorSeeds, _startRotateAxis);
            float startEndMoveTime = moveDuration - _completeRotateDuration;
            if (wastedTime > startEndMoveTime && _differentEndRotateAxis && _completeRotateDuration > 0)
            {
                endRotateTime += Time.deltaTime;
                var lerpToEndAxis = Vector3.Lerp(target.rotation.eulerAngles, Vector3.zero, wastedTime - startEndMoveTime / moveDuration - startEndMoveTime);
                frameDelta = -1 *  Vector3.Scale(lerpToEndAxis, _endRotateAxis.NormalizeInvert());
            }
            wastedTime += Time.deltaTime;
            var sourceRotation = target.rotation.eulerAngles;
            target.rotation = Quaternion.Euler(sourceRotation+frameDelta*Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator LocalRotate(Transform target, Transform targetParent)
    {
        float wastedTime = 0.0f;
        while (wastedTime <= _completeRotateDuration)
        {
            Quaternion frameDelta = Quaternion.Lerp(target.rotation, targetParent.rotation, wastedTime/_completeRotateDuration);
            target.rotation = frameDelta;
            wastedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = targetParent.rotation;
    }
    
    private float RandomSpeed()
    {       
        float speedRandomSpeed = _rotateSpeedRange.Random();
        return Random.Range(0, 2) == 0 ? speedRandomSpeed : -speedRandomSpeed;
    }
}

