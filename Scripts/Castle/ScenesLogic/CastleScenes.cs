using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class CastleScenes
{
    private static readonly string _sceneSaveName = "CurrentScene";
    public static string CutScene { get; } =  "CutScene";
    public static string GameScene { get; } = "GameScene";

    public static string GetCurrentSceneName()
    {
        return ES3.Load(_sceneSaveName, defaultValue:CutScene);
    }

    public static void ChangeCurrentGameScene(string sceneName)
    {
        ES3.Save(_sceneSaveName, sceneName);
    }

    public static string GetOpenedSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void LoadCurrentGameScene()
    {
        LoadScene(GetCurrentSceneName());
    }

    private static void LoadScene(string sceneName)
    {
        if (GetOpenedSceneName() == sceneName)
            return;
        SceneManager.LoadScene(sceneName);
    }


    public static void LoadWinterScene()
    {
        LoadScene("WinterScene");
    }
}
