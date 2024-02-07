using GameCore.Scripts.Tasks;
using UnityEngine;
using Zenject;

namespace GameCore.Scripts.Tiles.BuyZones
{
    public class WaterBuyZoneTileTask : ATask
    {
        [SerializeField] private Tile _tile;
        [SerializeField] private BuyZoneLinker _buyZoneLinker;

        [Inject] public Island Island { get; }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _tile.Watered.Once(() =>
            {
                CurrentValueChanged.Dispatch(1);
                Finished.Dispatch(this);
            });

            Island.Finished.Once(() =>
            {
                Finish();
            });
        }

        public override float GetCurrentProgress() => _tile.IsWatered ? 1 : 0;

        public override float GetFinishValue() => 1;

        public override float GetCurrentValue() => GetCurrentProgress();

        protected override bool IsFinishedInternal() => _tile.IsWatered;

        public override bool IsAvailableInternal() => IsFinishedInternal() == false 
                                                      && Island.BuyZonesController.IsZoneAvailable(_buyZoneLinker.Zone);
    }
}