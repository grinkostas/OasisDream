using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardTaker : MonoBehaviour
{
    [SerializeField] private RewardBooster _rewardBooster;
    [SerializeField] private RewardButton _rewardButton;

    private void OnEnable()
    {
        _rewardButton.Pressed.On(OnPressed);
    }

    private void OnDisable()
    {
        _rewardButton.Pressed.Off(OnPressed);
    }

    private void OnPressed()
    {
        _rewardBooster.Take();
    }
}
