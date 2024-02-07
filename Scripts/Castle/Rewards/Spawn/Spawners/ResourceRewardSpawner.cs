
public class ResourceRewardSpawner : BoosterRewardSpawner<ResourceRewardBooster, ResourceBoosterSpawnPoint>
{
    protected override void OnSpawnBooster(ResourceBoosterSpawnPoint spawnPoint)
    {
        var reward = spawnPoint.GetReward();
        CurrentBooster.SetReward(reward.Item1, reward.Item2);
    }
}
