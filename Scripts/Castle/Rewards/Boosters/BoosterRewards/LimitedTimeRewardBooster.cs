using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class LimitedTimeRewardBooster : RewardBooster
{
    public abstract float Duration { get; }
}
