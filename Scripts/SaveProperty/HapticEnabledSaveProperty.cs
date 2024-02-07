using StaserSDK.SaveProperties.Api;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties
{
    public class HapticEnabledSaveProperty : ES3SaveProperty<bool>
    {
        public override string key => "Haptic";
        public override bool defaultValue => true;
    }
}