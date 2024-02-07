using System;
using UnityEngine;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties.Api
{
    public static class PlayerPrefsSaveProperty
    {
        public static readonly SaveDomainRules saveDomainRules = new();
        public static bool IsAllowToSaveTo(string domain) => saveDomainRules.IsAllowToSaveTo(domain);
        
        public static readonly SaveDomainCollection saveDomainCollection = new();
    }
    
    public abstract class PlayerPrefsSaveProperty<T> : BaseSaveProperty<T>
    {
        public PlayerPrefsSaveProperty()
        {
            PlayerPrefsSaveProperty.saveDomainCollection.AddDomain(this, domain);
            // Call save when some changes in domain rules happen.
            // In the Save method additional check will be called.
            PlayerPrefsSaveProperty.saveDomainRules.onChange.On(Save, true); 
        }

        public override void Dispose()
        {
            base.Dispose();
            
            PlayerPrefsSaveProperty.saveDomainCollection.RemoveDomain(this, domain);
            PlayerPrefsSaveProperty.saveDomainRules.onChange.Off(Save); 
        }

        protected override void Remove()
        {
            PlayerPrefs.DeleteKey(key);
        }

        protected override bool IsAllowWritingTo(string domainName)
        {
            return PlayerPrefsSaveProperty.IsAllowToSaveTo(domainName);
        }

        protected override void OnDomainChange(string oldDomainName)
        {
            PlayerPrefsSaveProperty.saveDomainCollection.RemoveDomain(this, oldDomainName);
            PlayerPrefsSaveProperty.saveDomainCollection.AddDomain(this, domain);
        }
    }

    public abstract class DefaultPlayerPrefsSaveProperty<T> : PlayerPrefsSaveProperty<T>
    {
        private string _key;
        private string _domainAndKeyCached;
        public override string key
        {
            get
            {
                if (String.IsNullOrEmpty(_domainAndKeyCached)) _domainAndKeyCached = domain + _key;

                return _domainAndKeyCached;
            }
        }
        public override T defaultValue { get; }
        

        public DefaultPlayerPrefsSaveProperty(string key, T defaultValue = default, string domain = null)
        {
            _key = key;
            this.defaultValue = defaultValue;
            this.domain = domain;
        }

        protected override void OnDomainChange(string oldDomainName)
        {
            // Clean this value, so next when it called it will be done again.
            _domainAndKeyCached = null;
        }
    }
    
    public class BoolPlayerPrefsSaveProperty : DefaultPlayerPrefsSaveProperty<bool>
    {
        public BoolPlayerPrefsSaveProperty(string key, bool defaultValue = default, string domain = null) 
            : base(key, defaultValue, domain)
        {
        }
        
        protected override bool Read()
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) > 0;
        }

        protected override void Write()
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
    
    public class IntPlayerPrefsSaveProperty : DefaultPlayerPrefsSaveProperty<int>
    {
        public IntPlayerPrefsSaveProperty(string key, int defaultValue = default, string domain = null) 
            : base(key, defaultValue, domain)
        {
        }
        
        protected override int Read()
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        protected override void Write()
        {
            PlayerPrefs.SetInt(key, value);
        }
    }
    
    public class FloatPlayerPrefsSaveProperty : DefaultPlayerPrefsSaveProperty<float>
    {
        public FloatPlayerPrefsSaveProperty(string key, float defaultValue = default, string domain = null) 
            : base(key, defaultValue, domain)
        {
        }
        
        protected override float Read()
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        protected override void Write()
        {
            PlayerPrefs.SetFloat(key, value);
        }
    }
    
    public class StringPlayerPrefsSaveProperty : DefaultPlayerPrefsSaveProperty<string>
    {
        public StringPlayerPrefsSaveProperty(string key, string defaultValue = default, string domain = null) 
            : base(key, defaultValue, domain)
        {
        }
        
        protected override string Read()
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        protected override void Write()
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}