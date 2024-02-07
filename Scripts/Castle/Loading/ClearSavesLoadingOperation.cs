using System.IO;
using StaserSDK;
using StaserSDK.Loading;
using UnityEngine;

namespace GameCore.Scripts.Castle.Loading
{
    public class ClearSavesLoadingOperation : LoadingOperation
    {
        public override void Load()
        {
            if (Tutorial.Tutorial.Completed == false)
            {
                var previousScene = CastleScenes.GetCurrentSceneName();
                string persistentDataPath = Application.persistentDataPath;

                if (Directory.Exists(persistentDataPath))
                {
                    string[] files = Directory.GetFiles(persistentDataPath);

                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
                CastleScenes.ChangeCurrentGameScene(previousScene);
            }
            Finish();
        }
    }
}