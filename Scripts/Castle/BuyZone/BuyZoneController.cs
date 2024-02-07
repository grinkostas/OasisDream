using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyZoneController : MonoBehaviour
{
    private List<BuyZone> _buyZones = new();
    private List<object> _warningBlockers = new();

    public bool CanShowWarning => _warningBlockers.Count == 0;

    public void AddZone(BuyZone buyZone)
    {
        _buyZones.Add(buyZone);
    }

    public bool TreGetZone(string id, out BuyZone zone)
    {
        zone = null;
        if (_buyZones.Has(x => x.Id == id) == false)
            return false;
        zone = _buyZones.Find(x => x.Id == id);
        return true;
    }
    
    public BuyZone GetCurrentAvailableZone()
    {
        return _buyZones.Find(x => x.IsAvailable());
    }

    public void BlockWarning(object sender)
    {
        if(_warningBlockers.Contains(sender))
            return;
        _warningBlockers.Add(sender);
    }

    public void EnableWarnings(object sender)
    {
        _warningBlockers.Remove(sender);
    }
}
