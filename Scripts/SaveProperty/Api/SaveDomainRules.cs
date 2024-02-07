using System;
using System.Collections.Generic;
using NepixSignals;
using StaserSDK.Signals;
using StaserSDK.Utilities;
using UnityEngine;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties.Api
{
    public class SaveDomainRules
    {
        private TheSignal _onChange;
        public TheSignal onChange => _onChange ??= new();
        
        /// <summary>
        /// Used for performance optimization reasons.
        /// </summary>
        private bool _hasAnyBlocker;
        
        /// <summary>
        /// All domains including default blocker.
        /// Except allowed domains. <see cref="_alwaysAllowToSaveToDomains"/>
        /// </summary>
        private readonly ActionPermissionAuditor _allDomainsBlockers = new();
        public int GetBlockersCountForAllDomains()
        {
            return _allDomainsBlockers.count;
        }
        
        /// <summary>
        /// All blockers that can block saving to default domain.
        /// </summary>
        private readonly ActionPermissionAuditor _defaultDomainBlockers = new();
        public int GetBlockersCountForDefaultDomain()
        {
            return _defaultDomainBlockers.count;
        }

        /// <summary>
        /// Always allow to save to this domain.
        /// For instance: Cheat or Debug save properties could have own domain
        /// and we want to have ability to save to it.
        /// Note: into allowed domains can't be added <b>default</b> domain.
        /// </summary>
        private readonly HashSet<string> _alwaysAllowToSaveToDomains = new();

        public int GetCountAlwaysAllowToSaveToDomains() => _alwaysAllowToSaveToDomains.Count;
        public void ForEachAlwaysAllowToSaveDomains(Action<string> callback)
        {
            foreach (var domain in _alwaysAllowToSaveToDomains)
            {
                callback.Invoke(domain);
            }
        }

        /// <summary>
        /// Blocked domains that haven't permission to save.
        /// </summary>
        private readonly Dictionary<string, ActionPermissionAuditor> _blockedDomains = new();

        public int GetBlockersCountForDomain(string domain)
        {
            if (!_blockedDomains.ContainsKey(domain)) return 0;
            return _blockedDomains[domain].count;
        }

        private void ValidateHasAnyBlocker()
        {
            var blockedDomainsCount = 0;
            foreach (var pair in _blockedDomains)
            {
                if (pair.Value.hasBlockers) blockedDomainsCount++;
            }
            
            _hasAnyBlocker = _allDomainsBlockers.hasBlockers 
                             || _defaultDomainBlockers.hasBlockers 
                             || blockedDomainsCount > 0;
        }

        public void BlockAllDomainsExceptAllowed(object blockedId, bool single = false)
        {
            _allDomainsBlockers.Add(blockedId, single);
            _hasAnyBlocker = true;
            _onChange?.Dispatch();
        }
        public void UnblockAllDomains(object blockedId)
        {
            _allDomainsBlockers.Remove(blockedId);
            ValidateHasAnyBlocker();
            _onChange?.Dispatch();
        }

        public void BlockDefaultDomain(object blockedId, bool single = false)
        {
            _defaultDomainBlockers.Add(blockedId, single);
            _hasAnyBlocker = true;
            _onChange?.Dispatch();
        }
        public void UnblockDefaultDomain(object blockedId)
        {
            _defaultDomainBlockers.Remove(blockedId);
            ValidateHasAnyBlocker();
            _onChange?.Dispatch();
        }
        
        public void AlwaysAllowToSaveTo(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                Debug.LogError("Domain can't be null. Null domain means default domain.");
                return;
            }

            if (_blockedDomains.ContainsKey(domain))
            {
                Debug.LogError("Already added to blocked domain list.");
                return;
            }
            
            _alwaysAllowToSaveToDomains.Add(domain);
            _onChange?.Dispatch();
        }
        
        public void BlockSavingTo(string domain, object blockedId, bool single = false)
        {
            if (_alwaysAllowToSaveToDomains.Contains(domain))
            {
                Debug.LogError("Already added to allowed domain list.");
                return;
            }
            if (!_blockedDomains.ContainsKey(domain))
            {
                _blockedDomains.Add(domain, new());
            }
            _blockedDomains[domain].Add(blockedId, single);
            _hasAnyBlocker = true;
            _onChange?.Dispatch();
        }
        
        public void UnblockSavingTo(string domain, object blockedId)
        {
            if (!_blockedDomains.ContainsKey(domain)) return;
            _blockedDomains[domain].Remove(blockedId);
            ValidateHasAnyBlocker();
            _onChange?.Dispatch();
        }

        public bool IsAllowToSaveTo(string domain)
        {
            if (!_hasAnyBlocker) return true;
            
            // If domain is null that means this is default domain.
            // Note: Default domain should be on this place of checking!
            if (domain == null)
            {
                if (_defaultDomainBlockers.hasBlockers) return false;
                if (_allDomainsBlockers.hasBlockers) return false;
                return true;
            }
            
            // Check is the domain in a list of always allowed domains.
            if (_alwaysAllowToSaveToDomains.Contains(domain)) return true;
            
            // Check all domain blockers.
            if (_allDomainsBlockers.hasBlockers) return false;

            // Check domain blockers
            if (_blockedDomains.ContainsKey(domain) && _blockedDomains[domain].hasBlockers) return false;
            
            return true;
        }
    }
}