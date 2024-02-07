using System;
using TMPro;
using UnityEngine;

namespace GameCore.Scripts.Water
{
    public class WaterSourceMessagesDisplay : MonoBehaviour
    {
        [SerializeField] private WaterGiver _waterGiver;
        [SerializeField] private View _messageView;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Transform _viewWrapper;
        [SerializeField] private Vector3 _delta;

        private void OnEnable()
        {
            _waterGiver.NewMessage.On(OnMessageReceived);
        }

        private void OnDisable()
        {
            _waterGiver.NewMessage.Off(OnMessageReceived);
        }

        private void OnMessageReceived(EquippedCharacter character, string message)
        {
            _viewWrapper.position = character.transform.position + _delta;
            _messageText.text = message;
            _messageView.Show();
        }
    }
}