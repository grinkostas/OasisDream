using StaserSDK.Stack;

namespace GameCore.Scripts.MarketLogic.Customers
{
    public class Request
    {
        public ItemType Type { get; }
        public int Amount { get; }

        public Request(ItemType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}