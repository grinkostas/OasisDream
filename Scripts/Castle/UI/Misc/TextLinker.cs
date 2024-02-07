using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextLinker : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _textToSet;

    private void Awake()
    {
        _text.text = _textToSet;
    }
}
