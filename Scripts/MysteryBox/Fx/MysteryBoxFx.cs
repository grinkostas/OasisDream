using UnityEngine;

namespace GameCore.Scripts.MysteryBox
{
    public class MysteryBoxFx : MonoBehaviour
    {
        [SerializeField] private BoxPunching _boxPunching;
        [SerializeField] private SpringboardFx _springboardFx;
        [SerializeField] private PlayerJumpFx _playerJumpFx;
        [SerializeField] private MysteryBoxTrigger _mysteryBoxTrigger;
        
        private void OnEnable()
        {
            _mysteryBoxTrigger.Triggered.On(OnTriggered);
        }

        private void OnDisable()
        {
            _mysteryBoxTrigger.Triggered.Off(OnTriggered);
        }

        private void OnTriggered()
        {
            _boxPunching.Punch();
            _playerJumpFx.Jump();
            _springboardFx.Force();
        }
    }
}