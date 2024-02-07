using StaserSDK;

namespace GameCore.Scripts.Castle.Tutorial
{
    public static class Tutorial
    {
        public static bool Completed => ES3.Load(Constants.TutorialFinishSaveId, false);

        public static void Complete()
        {
            ES3.Save(Constants.TutorialFinishSaveId, true);
        }
    }
}