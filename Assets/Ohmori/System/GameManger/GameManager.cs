using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private float _readySeconds = 3f;

    public float CurrentReadySeconds { get; private set; }

    public float CurrentGameElapsedSeconds { get; private set; }

    private Coroutine _currentGame = null;

    private void PlayInit()
    {
        CurrentReadySeconds = 0F;
        CurrentGameElapsedSeconds = 0F;
    }

    private void PublichGameStart()
    {
        var currentScene = SceneManager.GetActiveScene();

        foreach (var root in currentScene.GetRootGameObjects())
        {
            var recievers = root.GetComponentsInChildren<IGameStartReciever>();

            foreach (var reciever in recievers)
            {
                reciever.GameStart();
            }
        }
    }

    private IEnumerator Ready()
    {
        while (CurrentReadySeconds < _readySeconds)
        {
            CurrentReadySeconds += Time.deltaTime;

            yield return null;
        }

        CurrentReadySeconds = _readySeconds;
    }

    private IEnumerator Play()
    {
        PublichGameStart();

        while (true)
        {
            CurrentGameElapsedSeconds = Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator GamePlay()
    {
        // 初期化
        PlayInit();

        // 最初の指定秒数待つ処理
        yield return Ready();

        // ゲームスタート
        yield return Play();
    }

    ///<summary>
    ///指定秒待ってゲームを再生する
    ///</summary>
    public void PlayReady()
    {
        if (_currentGame is not null) return;

        _currentGame = StartCoroutine(GamePlay());
    }

    private void PublichGameEnd()
    {
        var currentScene = SceneManager.GetActiveScene();

        foreach (var root in currentScene.GetRootGameObjects())
        {
            var recievers = root.GetComponentsInChildren<IGameEndReciever>();

            foreach (var reciever in recievers)
            {
                reciever.GameEnd();
            }
        }
    }

    public void StopGame()
    {
        if (_currentGame is null) return;

        StopCoroutine(_currentGame);
        _currentGame = null;
    }
}
