using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoosterModel : MonoBehaviour
{
    [SerializeField] private float _targetScale;
    public void Init(RewardBooster booster)
    {
        transform.SetParent(booster.ModelParent);
        transform.localPosition = Vector3.zero;
        transform.localScale = _targetScale * Vector3.one;
        gameObject.SetActive(true);
    }
}
