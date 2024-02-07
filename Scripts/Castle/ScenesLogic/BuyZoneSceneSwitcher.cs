using System;
using UnityEngine;


public class BuyZoneSceneSwitcher : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private NextSceneFx _nextSceneFx;
    
    private void OnEnable()
    {
        if (_buyZone.IsBoughtCheck())
        {
            Switch();
            return;
        }
        _buyZone.Bought.On(Switch);
    }

    private void OnDisable()
    {
        _buyZone.Bought.Off(Switch);
    }

    private void Switch()
    {
        _nextSceneFx.Show();
    }
}
