using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Castle.CheatView
{
    public class WaterIslandCheat : CheatButtonBase
    {
        [Inject] public IslandsController IslandsController { get; }
        public override void OnButtonClicked()
        {
            foreach (var tile in IslandsController.GetCurrentIsland().Tiles)
            {
                tile.EnableGrass();
            }
        }
    }
}