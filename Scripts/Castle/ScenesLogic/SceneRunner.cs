using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneRunner : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public void Load()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
