using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Scripts.Castle.CheatView.Logic
{
    public class CheatContainer : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorialObject;
        [SerializeField] private GameObject _bridgeToVulcanoIsland;
        [SerializeField] private GameObject _bridgeWrapperToVulcanoIsland;
        [SerializeField] private BuyZone _sandBringeZone;

        private List<GameObject> _storedObjectsCached;
        public List<GameObject> StoredObjects
        {
            get
            {
                if (_storedObjectsCached == null || _storedObjectsCached.Count == 0)
                    _storedObjectsCached = new List<GameObject> { _tutorialObject, _bridgeToVulcanoIsland, _bridgeWrapperToVulcanoIsland };
                return _storedObjectsCached;
            }
        }
        
        private List<BuyZone> _storedBuyZonesCached;
        public List<BuyZone> StoredBuyZones
        {
            get
            {
                if (_storedBuyZonesCached == null || _storedBuyZonesCached.Count == 0)
                    _storedBuyZonesCached = new List<BuyZone> { _sandBringeZone };
                return _storedBuyZonesCached;
            }
        }
        

    }
}