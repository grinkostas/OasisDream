using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skin : MonoBehaviour
{
    [SerializeField] private View _skinView;
    [SerializeField] private SkinData _skinData;
    
    public string Id => _skinData.Id;
    public bool Available => _skinData.Available;

    public void Select()
    {
        _skinView.Show();
    }

    public void Deselect()
    {
         _skinView.Hide();
    }
    
}
