using NepixSDK.NepixCore.Plugins.EasySave3.Nepix;
using StaserSDK.SaveProperties.SaveData.Base;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties.SaveData
{
    public class UpgradePropertySaveData : ES3SavePropertyData<UpgradePropertySaveData.InternalSaveData>
    {
        public override string key { get; }

        public UpgradePropertySaveData(string key)
        {
            this.key = key;
        }

        [ES3Alias("UpgradePropertySaveData")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable]
            private int _level = 1;
            public int level
            {
                get => _level;
                set
                {
                    _level = value;
                    DispatchChange();
                }
            }
            
            [ES3Serializable]
            private int _milestone;
            public int milestone
            {
                get => _milestone;
                set
                {
                    _milestone = value;
                    DispatchChange();
                }
            }
        }
    }
}