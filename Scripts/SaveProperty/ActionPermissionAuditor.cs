using System.Collections.Generic;
using UnityEngine;

namespace StaserSDK.Utilities
{
    public class ActionPermissionAuditor
    {
        private readonly HashSet<object> _blockers = new();
        public bool hasBlockers => _blockers.Count != 0;
        public int count => _blockers.Count;

        public void Add(object blocker, bool single = true)
        {
            if (_blockers.Contains(blocker) && single) return;
            
            _blockers.Add(blocker);
        }

        public void Remove(object blocker)
        {
            _blockers.Remove(blocker);
        }
    }
    
}