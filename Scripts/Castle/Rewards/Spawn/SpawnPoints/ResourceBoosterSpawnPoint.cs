using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK.Stack;

public class ResourceBoosterSpawnPoint : BoosterSpawnPoint
{
    [SerializeField] private ItemType _rewardType;
    [SerializeField] private Vector2Int _rewardRange;
  
    public (ItemType, int) GetReward()
    {
        return (_rewardType, _rewardRange.Random());
    }

    public override void AddPointToController()
    {
        BoostersController.AddPoint(this);
    }

    public override void RemovePointController()
    {
        BoostersController.RemovePoint(this);
    }
}
