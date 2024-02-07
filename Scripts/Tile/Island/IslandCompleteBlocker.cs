using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.Scripts.Tiles.BuyZones
{
    public class IslandCompleteBlocker : MonoBehaviour
    {
        [SerializeField] private Transform _targetWrapper;
        [SerializeField] private Transform _disabledWrapper;

        [Inject] public Island Island { get; }
        
        private void OnEnable()
        {
            _targetWrapper.gameObject.SetActive(false);
            _disabledWrapper.gameObject.SetActive(true);
            if (Island.IsCompleted == false)
                Island.Completed.Once(Activate);
            else 
                Activate();
        }

        private void Activate()
        {
            _targetWrapper.gameObject.SetActive(true);
            _disabledWrapper.gameObject.SetActive(false);
        }
    }
}