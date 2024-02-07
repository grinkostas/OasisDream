using Zenject;

namespace GameCore.Scripts.Castle.CheatView
{
    public class InfiniteStackCheatButton : CheatButtonBase
    {
        [Inject] public Player Player { get; }
        public override void OnButtonClicked()
        {
            Player.Stack.MainStack.MakeInfinite();
            Player.Stack.SoftCurrencyStack.MakeInfinite();
        }
    }
}