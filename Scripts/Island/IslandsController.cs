using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Scripts
{
    public class IslandsController : MonoBehaviour
    {
        [SerializeField] private List<Island> _islandsQueue;

        public List<Island> Islands => _islandsQueue;

        public Island GetCurrentIsland()
        {
            return _islandsQueue.Find(x => x.IsFinished == false && x.gameObject.activeInHierarchy);
        }
    }
}