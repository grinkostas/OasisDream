using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.Scripts
{
    public class BuyZoneLinker : MonoBehaviour
    {
        [SerializeField, HideIf(nameof(_getFromChild))] private BuyZone _buyZone;
        [SerializeField] private bool _getFromChild = true;

        public BuyZone Zone => _getFromChild ?  GetFromParent() : _buyZone;

        private BuyZone _buyZoneCached;

        private BuyZone GetFromParent()
        {
            if (_buyZoneCached == null)
            {
                _buyZoneCached = GetComponentInChildren<BuyZone>(true);
                if(_buyZoneCached == null)
                    Debug.Log("Have no buy zones in children");
            }

            return _buyZoneCached;
        }
    }
}