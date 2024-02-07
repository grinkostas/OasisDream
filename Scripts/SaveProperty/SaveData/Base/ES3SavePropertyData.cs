using StaserSDK.SaveProperties.Api;

namespace StaserSDK.SaveProperties.SaveData.Base
{
    #pragma warning disable 0649
    
    /// <summary>
    /// Inherit this class if you want to save none-primitive types
    /// and dispatch changes.
    /// </summary>
    /// <typeparam name="TSaveData"></typeparam>
    public abstract class ES3SavePropertyData<TSaveData> : ES3SaveProperty<TSaveData>
        where TSaveData : ES3SaveData<TSaveData>, new()
    {
        private TSaveData _defaultValue;
        public override TSaveData defaultValue => _defaultValue ??= CreateDefault();

        protected override void OnLoad() => value.SetSaveProperty(this);

        protected virtual TSaveData CreateDefault() => new ();
    }
}