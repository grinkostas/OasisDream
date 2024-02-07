using GameCore.Scripts.Tasks;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Tiles
{
    public class WaterIslandTask : ATask
    {
        [Inject] public Island Island { get; }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Island.WateredTile.On(() =>
            {
                CurrentValueChanged.Dispatch(Island.WateredTiles);
                if(IsFinishedInternal())
                    Finished.Dispatch(this);
            });
        }
        
        public override float GetCurrentProgress() => Island.WateredTiles / GetFinishValue();
        public override float GetFinishValue() => Island.MinTilesToComplete;
        public override float GetCurrentValue() => Island.WateredTiles;
        protected override bool IsFinishedInternal() => Island.WateredTiles >= Island.MinTilesToComplete;
        public override bool IsAvailableInternal() => IsFinishedInternal() == false;
    }
}