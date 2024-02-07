using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Scripts.Castle.CheatView.Logic
{
    public class CheatButtonGroup : CheatButtonBase
    {
        [SerializeField] private List<CheatButtonBase> _cheatButtonBases;
        public override void OnButtonClicked()
        {
            foreach (var buttonBase in _cheatButtonBases)
            {
                Debug.Log($"{buttonBase.gameObject.name} clicked");
                buttonBase.OnButtonClicked();
            }
        }
    }
}