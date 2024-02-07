using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardButtonData
{
    public bool SkipReward { get; }

    public RewardButtonData(bool skipReward = false)
    {
        SkipReward = skipReward;
    }
}
