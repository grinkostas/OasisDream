using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class ModifiersSettings : Settings<ModifiersSettings>
{
    public float IncreaseSpeedDuration;
    public float MaxStackDuration;
    public float InfiniteTorceDuration;
    public float RecycleBoostDuration = 120f;
    public float HelpersBoostDuration = 120f;
    public float WeaponUpgradeDuration = 120f;
}
