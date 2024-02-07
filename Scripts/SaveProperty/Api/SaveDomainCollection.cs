using System;
using System.Collections.Generic;
using System.Linq;
using NepixSignals;
using StaserSDK.Signals;
using StaserSDK.Utilities;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties.Api
{
    public class SaveDomainCollection
    {
        public TheSignal<string> onChange { get; } = new();
        private Dictionary<string, ActionPermissionAuditor> allDomains { get; } = new();

        public List<string> GetAllDomainNames()
        {
            return allDomains.Select(pair => pair.Key).ToList();
        }

        public void AddDomain(BaseSaveProperty saveProperty, string domain)
        {
            if (string.IsNullOrEmpty(domain)) return;
            if (!allDomains.ContainsKey(domain))
            {
                allDomains.Add(domain, new());
            }

            var auditor = allDomains[domain];
            auditor.Add(saveProperty, true);
            onChange.Dispatch(domain);
        }
        
        public void RemoveDomain(BaseSaveProperty saveProperty, string domain)
        {
            if (string.IsNullOrEmpty(domain)) return;
            if (!allDomains.ContainsKey(domain)) return;
            
            var auditor = allDomains[domain];
            auditor.Remove(saveProperty);

            if (!auditor.hasBlockers) allDomains.Remove(domain);
            onChange.Dispatch(domain);
        }
    }
}