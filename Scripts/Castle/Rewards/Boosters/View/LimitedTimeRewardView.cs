using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LimitedTimeRewardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private LimitedTimeRewardBooster _limitedTimeRewardBooster;

    private void OnEnable()
    {
        _text.text = $"{_limitedTimeRewardBooster.Duration}s";
    }
}
