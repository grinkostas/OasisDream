using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour
{
    [SerializeField] private Image _graphic;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Button _button;

    public Button Button => _button;

    public void Select()
    {
        _graphic.color = _selectedColor;
    }

    public void Deselect()
    {
        _graphic.color = _defaultColor;
    }

}
