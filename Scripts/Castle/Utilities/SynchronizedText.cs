using System;
using TMPro;
using UnityEngine;

namespace GameCore.Scripts.Utilities
{
    [RequireComponent(typeof(TMP_Text))]
    public class SynchronizedText : MonoBehaviour
    {
        [SerializeField] private TextSynchronizer _textSynchronizer;
        private InitializedProperty<TMP_Text> _text;

        private void Awake()
        {
            _text = new InitializedProperty<TMP_Text>(() => gameObject.GetComponent<TMP_Text>());
        }

        private void OnEnable()
        {
            OnTextChanged(_textSynchronizer.text);
            _textSynchronizer.TextChanged.On(OnTextChanged);
        }

        private void OnDisable()
        {
            _textSynchronizer.TextChanged.On(OnTextChanged);
        }

        private void OnTextChanged(string value)
        {
            _text.Value.text = value;
        }
    }
}