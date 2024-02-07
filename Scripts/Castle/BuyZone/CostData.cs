
using StaserSDK.Stack;

[System.Serializable]
public class CostData
{
    public ItemType Resource;
    public int Amount;

    public void Spend(int amount)
    {
        Amount -= amount;
    }

    public CostData Copy()
    {
        return new CostData() { Resource = Resource, Amount = Amount };
    }
}
