using System;
using NepixSignals;
using StaserSDK.Signals;
using UnityEngine;

namespace StaserSDK.Utilities
{
    public class OnDestroyListener : MonoBehaviour
    {
        public TheSignal onDestroy { get; } = new();
        
        private void OnDestroy()
        {
            onDestroy?.Dispatch();
        }
    }
}