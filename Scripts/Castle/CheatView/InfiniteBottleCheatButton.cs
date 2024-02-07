using Zenject;

namespace GameCore.Scripts.Castle.CheatView
{
    public class InfiniteBottleCheatButton : CheatButtonBase
    {
        [Inject] public Player Player { get; }
        public override void OnButtonClicked()
        {
            Player.WaterBottle.MakeInfinite();
        }
    }
}