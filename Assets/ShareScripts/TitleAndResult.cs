using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "GameScene";

    public void GoNextScene()
    {
        SingletonSceneManager.I.LoadScene(_gameSceneName);
    }
}
