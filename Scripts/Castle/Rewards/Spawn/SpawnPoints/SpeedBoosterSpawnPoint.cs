using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedBoosterSpawnPoint : BoosterSpawnPoint
{
    public override void AddPointToController()
    {
        BoostersController.AddPoint(this);
    }

    public override void RemovePointController()
    {
        BoostersController.RemovePoint(this);
    }
    
}
