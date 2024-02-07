using System;
using UnityEngine;


public class BuyZoneModel : MonoBehaviour
{
    [SerializeField] private BuyZone _buyZone;
    [SerializeField] private View _modelView;

    private void OnEnable()
    {
        if (_buyZone.IsBoughtCheck())
        {
            _modelView.Hide();
            return;
        }

        _buyZone.Bought.On(_modelView.Hide);
    }
}
