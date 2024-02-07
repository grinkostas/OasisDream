using GameCore.Scripts.UI.Canvases;
using GameCore.Scripts.Utilities;
using UnityEngine;

namespace GameCore.Scripts.Stack
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(WorldSpaceToCanvasSpaceConverter))]
    public class CanvasItem : MonoBehaviour, IPoolItem<CanvasItem>
    {
        private RectTransform _rectTransformCached;
        public RectTransform Rect
        {
            get
            {
                if (_rectTransformCached == null)
                    _rectTransformCached = gameObject.GetComponent<RectTransform>();
                return _rectTransformCached;
            }
        }
        
        private CanvasGroup _canvasGroupCached;
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroupCached == null)
                    _canvasGroupCached = gameObject.GetComponent<CanvasGroup>();
                return _canvasGroupCached;
            }
        }
        
        private WorldSpaceToCanvasSpaceConverter _worldSpaceToCanvasSpaceConverterCached;
        public WorldSpaceToCanvasSpaceConverter WorldSpaceToCanvasSpaceConverter
        {
            get
            {
                if (_worldSpaceToCanvasSpaceConverterCached == null)
                    _worldSpaceToCanvasSpaceConverterCached = gameObject.GetComponent<WorldSpaceToCanvasSpaceConverter>();
                return _worldSpaceToCanvasSpaceConverterCached;
            }
        }

        public IPool<CanvasItem> Pool { get; set; }
        public bool IsTook { get; set; }
        public void TakeItem()
        {
            gameObject.SetActive(true);
        }

        public void ReturnItem()
        {
            gameObject.SetActive(false);
        }
    }
}