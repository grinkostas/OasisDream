using System.Collections.Generic;
using GameCore.Scripts.Tasks;
using GameCore.Scripts.Tiles;
using UnityEngine;

namespace GameCore.Scripts.Castle.Tutorial.Tasks
{
    public class WaterTileTask : ATask
    {
        [SerializeField] private List<Tile> _tiles;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (IsFinished())
            {
                Finish();
                return;
            }

            foreach (var tile in _tiles)
                tile.Watered.On(OnWatered);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (var tile in _tiles)
                tile.Watered.Off(OnWatered);
        }

        private void OnWatered()
        {
            Finish();
        }

        public override float GetCurrentProgress() => GetCurrentValue();

        public override float GetFinishValue() => 1;

        public override float GetCurrentValue() => IsFinished() ? 1 : 0;

        protected override bool IsFinishedInternal() => _tiles.Has(x => x.IsWatered);

        public override bool IsAvailableInternal() => !IsFinished();
    }
}