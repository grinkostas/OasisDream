#define ES3_SAVINGS
using UnityEngine;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties.Api
{
    public static class ES3SaveProperty
    {
        /// <summary>
        /// Save domain rules
        /// </summary>
        public static readonly SaveDomainRules saveDomainRules = new();
        public static bool IsAllowToSaveTo(string domain) => saveDomainRules.IsAllowToSaveTo(domain);
        
        /// <summary>
        /// Save domain collection
        /// </summary>
        public static readonly SaveDomainCollection saveDomainCollection = new();
    }

    public abstract class ES3SaveProperty<T> : BaseSaveProperty<T>
    {
        private string _fileExtOfDomain = ".data";
        public string fileExtOfDomain
        {
            get => _fileExtOfDomain;
            protected set
            {
                if (_fileExtOfDomain == value) return;
                _fileExtOfDomain = value;
                OnDomainChange(domain);
            }
        }

        private readonly ES3Settings _es3Settings;

        public ES3SaveProperty()
        {
            var encryption = ES3.EncryptionType.None;
            _es3Settings = new(GetDomainWithFileExt(), encryption);
            
            ES3SaveProperty.saveDomainCollection.AddDomain(this, domain);
            // Call save when some changes in domain rules happen.
            // In the Save method additional check will be called.
            ES3SaveProperty.saveDomainRules.onChange.On(Save, true); 
        }

        public override void Dispose()
        {
            base.Dispose();
            
            ES3SaveProperty.saveDomainCollection.RemoveDomain(this, domain);
            ES3SaveProperty.saveDomainRules.onChange.Off(Save); 
        }
        
        

        protected override T Read()
        {
            return ES3.Load(key, defaultValue, _es3Settings);
        }

        protected override void Write()
        {
            ES3.Save(key, _value, _es3Settings);
        }

        protected override void Remove()
        {
            ES3.DeleteKey(key, _es3Settings);
        }

        protected override bool IsAllowWritingTo(string domainName)
        {
            return ES3SaveProperty.IsAllowToSaveTo(domainName);
        }

        protected override void OnDomainChange(string oldDomainName)
        {
            _es3Settings.path = GetDomainWithFileExt();
            if (oldDomainName != domain)
            {
                ES3SaveProperty.saveDomainCollection.RemoveDomain(this, oldDomainName);
                ES3SaveProperty.saveDomainCollection.AddDomain(this, domain);
            }
        }

        private string GetDomainWithFileExt() => string.IsNullOrEmpty(domain) ? null : RefineDomain(domain);
        private string RefineDomain(string currDomain) => $"Saves/" + currDomain + fileExtOfDomain;
    }

    public class TheSaveProperty<T> : ES3SaveProperty<T>
    {
        public override string key { get; }
        public override T defaultValue { get; }

        public TheSaveProperty(string key, T defaultValue = default, string domain = null, GameObject linkToDispose = null)
        {
            this.key = key;
            this.defaultValue = defaultValue;
            this.domain = domain;
        }
    }
}