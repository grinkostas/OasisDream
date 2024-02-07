using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeScaleChanger : MonoBehaviour
{
    [SerializeField] private float _targetTimeScale = 1.0f;

    private void Update()
    {
        Time.timeScale = _targetTimeScale;
    }
}
