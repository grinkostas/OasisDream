namespace StaserSDK.SaveProperties.SaveData.Base
{
    #pragma warning disable 0649
    public abstract class ES3SaveData<T> where T : ES3SaveData<T>, new()
    {
        private ES3SavePropertyData<T> _saveProperty;

        public void SetSaveProperty(ES3SavePropertyData<T> value) => _saveProperty = value;

        protected void DispatchChange()
        {
            _saveProperty.SetDirty();
            _saveProperty.Save();
            _saveProperty.onChange.Dispatch(this as T);
        }
    }
}