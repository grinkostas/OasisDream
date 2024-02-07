using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBoostersSpawner : BoosterRewardSpawner<WeaponDamageBooster, WeaponBoosterSpawnPoint>
{
    protected override void OnSpawnBooster(WeaponBoosterSpawnPoint spawnPoint)
    {
        CurrentBooster.Init(spawnPoint.WeaponType, spawnPoint.IntModifier, spawnPoint.Duration);
        if(spawnPoint.ChangeSkin)
            CurrentBooster.SetSkin(spawnPoint.ItemForSkinId, spawnPoint.Skin);
    }
}
