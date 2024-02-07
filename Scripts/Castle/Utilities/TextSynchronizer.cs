using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NepixSignals;
using TMPro;

namespace GameCore.Scripts.Utilities
{
    public class TextSynchronizer : MonoBehaviour
    {
        private string _text;
        public string text
        {
            get => _text;
            set
            {
                _text = value;
                TextChanged.Dispatch(value);
            }
        }


        public TheSignal<string> TextChanged { get; } = new();
    }
}