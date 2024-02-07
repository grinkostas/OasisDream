using StaserSDK.SaveProperties.Api;

#pragma warning disable 0649
namespace StaserSDK.SaveProperties
{
    public class GameComplexityTunerBiasSaveProperty : ES3SaveProperty<float>
    {
        public override string key => "GameComplexityTunerBias";

        public override float defaultValue => 1;
    }
}