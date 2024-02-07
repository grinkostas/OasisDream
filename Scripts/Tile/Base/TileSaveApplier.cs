using UnityEngine;

namespace GameCore.Scripts.Tiles
{
    public class TileSaveApplier : MonoBehaviour
    {
        [SerializeField] private GameObject _wateredModel;
        [SerializeField] private GameObject _smallWateredModels;
        [SerializeField] private Tile _tile;
        [SerializeField] private GameObject _finalModel;
        [SerializeField] private TileSwapModel _tileSwapModel;
        
        private void Awake()
        {
            if(_tile.IsWatered == false)
                return;
            _tileSwapModel.enabled = false;
            _smallWateredModels.SetActive(false);
            _wateredModel.SetActive(true);
            _finalModel.SetActive(true);
        }
        
    }
}