using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SingletonSceneManager : SingletonMonoBehaviour<SingletonSceneManager>
{
    [SerializeField]
    private DissolveSceneChanger _sceneChanger;

    // ロードをする
    [ContextMenu("Test")]
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    private void PublishLoadEnd()
    {
        var currentScene = SceneManager.GetActiveScene();

        foreach (var root in currentScene.GetRootGameObjects())
        {
            var recievers = root.GetComponentsInChildren<ISceneLoadEndReciever>();

            foreach (var reciever in recievers)
            {
                reciever.SceneLoadEnd();
            }
        }
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return _sceneChanger.BeforeDissolve();

        SceneManager.LoadScene(sceneName);
        
        yield return _sceneChanger.DissolveAnimation();
        
        PublishLoadEnd();
    }
}
