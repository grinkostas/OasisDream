using UnityEngine;

namespace GameCore.Scripts.Utilities
{
    public class MonoItem : MonoBehaviour, IPoolItem<MonoItem>
    {
        public IPool<MonoItem> Pool { get; set; }
        public bool IsTook { get; set; }
        
        public void TakeItem()
        {
            gameObject.SetActive(true);
        }

        public void ReturnItem()
        {
            gameObject.SetActive(false);
        }
    }
}