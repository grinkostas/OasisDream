using UnityEngine;
using Zenject;

namespace GameCore.Scripts
{
    public class PlayerIslandPlacer : MonoBehaviour
    {
        [SerializeField] private IslandsController _islandsController;
        [Inject] public Player Player { get; }

        private void Awake()
        {
            Player.transform.position = _islandsController.GetCurrentIsland().SpawnPoint.position;
        }
    }
}