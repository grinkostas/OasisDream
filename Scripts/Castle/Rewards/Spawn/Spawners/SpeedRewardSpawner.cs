using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

public class SpeedRewardSpawner : BoosterRewardSpawner<SpeedRewardBooster, SpeedBoosterSpawnPoint>
{
    [Inject, UsedImplicitly] public ModifiersController modifiersController { get; }
    
    protected override bool NeedToSpawn()
    {
        return modifiersController.CurrentModifiers.Has(x => x is SpeedTimeModifier) == false;
    }
}
