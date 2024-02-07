using System;
using NepixSignals;
using StaserSDK.Signals;
using StaserSDK.Utilities;
using Unity.VisualScripting;
using UnityEngine;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties.Api
{
    public abstract class BaseSaveProperty : IDisposable
    {
        public abstract string key { get; }
        public abstract object valueObj { get; }
        public virtual void Dispose()
        {
        }

        public void LinkToDispose(GameObject gameObject)
        {
            gameObject.GetOrAddComponent<OnDestroyListener>().onDestroy.Once(Dispose);
        }
    }
    
    public abstract class BaseSaveProperty<T> : BaseSaveProperty
    {
        public override object valueObj => value;
        protected T _value;

        public T value
        {
            get
            {
                if (!_loaded) Load();
                return _value;
            }
            set
            {
                _value = value;
                SetDirty();
                Save();
                onChange.Dispatch(_value);
            }
        }
        
        public abstract T defaultValue { get; }

        private string _domain;
        public string domain
        {
            get => _domain;
            protected set
            {
                if (_domain == value) return;
                var oldDomainName = _domain;
                _domain = value;
                OnDomainChange(oldDomainName);
            }
        }

        private bool _allowWriting = true;
        public bool allowWriting
        {
            get => _allowWriting;
            set
            {
                _allowWriting = value;
                if (_allowWriting) Save();
            }
        }

        public readonly TheSignal<T> onChange = new();

        private bool _loaded;
        private bool _dirty;

        public void SetDirty()
        {
            _dirty = true;
        }
        
        public void Save()
        {
            if (!_dirty) return;
            if (!allowWriting) return;
            if (!IsAllowWritingTo(domain)) return;

            Write();
            OnSave();
        }

        public void Load()
        {
            _loaded = true;
            try
            {
                _value = Read();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            OnLoad();
        }

        public void Delete(bool resetToDefault = true)
        {
            if (resetToDefault) _value = defaultValue;
            OnLoad();
            
            // Deleting is also the operation of writing
            // that's why we need to check it.
            if (!allowWriting) return;
            if (!IsAllowWritingTo(domain)) return;
            Remove();
        }

        protected abstract T Read();
        protected abstract void Write();
        protected abstract void Remove();
        protected abstract bool IsAllowWritingTo(string domainName);
        
        protected virtual void OnSave() { }
        protected virtual void OnLoad() { }
        protected virtual void OnDomainChange(string oldDomainName) { }
    }
}