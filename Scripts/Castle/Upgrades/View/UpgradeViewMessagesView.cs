using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UpgradeViewMessagesView : MonoBehaviour
{
    [SerializeField] private View _view;
    [SerializeField] private TMP_Text _messageText;

    public void NotEnoughCoins()
    {
        _messageText.text = "Not enough Gold";
        _view.Show();
    }

    public void Max()
    {
        _messageText.text = "Max Level";
        _view.Show();
    }
}
