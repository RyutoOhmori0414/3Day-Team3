using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SingletonSceneManager : SingletonMonoBehaviour<SingletonSceneManager>
{
    [SerializeField]
    private DissolveSceneChanger _sceneChanger;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
