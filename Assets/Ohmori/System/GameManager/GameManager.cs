using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField]
    private float _readySeconds = 3f;

    [SerializeField] private float _finishTime = 60f;

    public float CurrentReadySeconds { get; private set; }
    public float CurrentGameElapsedSeconds { get; private set; }

    public int _defeatEnemyCount = 0;
    private int _score = 0;

    /// <summary>現在の倒したEnemy</summary>
    public int DefeatEnemyCount => _defeatEnemyCount;
    /// <summary>現在のスコア</summary>
    public int Score => _score;

    /// <summary>エネミーが増加した際に呼ばれて、引数として現在の倒した敵の数が渡されます</summary>
    public event Action<int> OnEnemyDefeated;
    /// <summary>スコアが増えた際に呼ばれます、引数として現在のスコアが渡されます</summary>
    public event Action<int> OnScoreChanged;

    protected override void OnThisDestroy()
    {
        base.OnThisDestroy();

        OnEnemyDefeated = null;
        OnScoreChanged = null;
    }

    /// <summary>倒したEnemyが一体増えます</summary>
    public void DefeatEnemy()
    {
        _defeatEnemyCount++;

        OnEnemyDefeated?.Invoke(_defeatEnemyCount);
    }

    /// <summary>スコアを追加します</summary>
    /// <param name="addScore">追加するスコア</param>
    public void AddScore(int addScore)
    {
        _score += addScore;

        OnScoreChanged?.Invoke(_score);
    }

    /// <summary>カウントと登録した関数をリセットします。</summary>
    public void ResetParams()
    {
        _defeatEnemyCount = 0;
        _score = 0;

        OnEnemyDefeated = null;
        OnScoreChanged = null;
    }

    private Coroutine _currentGame = null;

    private void PublishGameReady()
    {
        var currentScene = SceneManager.GetActiveScene();

        foreach (var root in currentScene.GetRootGameObjects())
        {
            var recievers = root.GetComponentsInChildren<IGameReadyReciever>(true);

            foreach (var reciever in recievers)
            {
                reciever.GameReady();
            }
        }
    }

    private void PublishGameStart()
    {
        var currentScene = SceneManager.GetActiveScene();

        foreach (var root in currentScene.GetRootGameObjects())
        {
            var recievers = root.GetComponentsInChildren<IGameStartReciever>(true);

            foreach (var reciever in recievers)
            {
                reciever.GameStart();
            }
        }
    }

    private void PublishGameEnd()
    {
        var currentScene = SceneManager.GetActiveScene();

        foreach (var root in currentScene.GetRootGameObjects())
        {
            var recievers = root.GetComponentsInChildren<IGameEndReciever>(true);

            foreach (var reciever in recievers)
            {
                reciever.GameEnd();
            }
        }
    }

    private void PlayInit()
    {
        CurrentReadySeconds = 0F;
        CurrentGameElapsedSeconds = 0F;
    }

    private IEnumerator Ready()
    {
        CurrentReadySeconds = _readySeconds;
        
        while (CurrentReadySeconds > 0)
        {
            CurrentReadySeconds -= Time.deltaTime;

            yield return null;
        }

        CurrentReadySeconds = 0;
    }

    private IEnumerator Play()
    {
        PublishGameStart();
        CurrentGameElapsedSeconds = _finishTime;

        while (CurrentGameElapsedSeconds > 0)
        {
            CurrentGameElapsedSeconds -= Time.deltaTime;

            yield return null;
        }

        CurrentGameElapsedSeconds = 0.0f;
        StopGame();
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

        ResetParams();
        PublishGameReady();
        _currentGame = StartCoroutine(GamePlay());
    }

    public void StopGame()
    {
        if (_currentGame is null) return;

        StopCoroutine(_currentGame);
        _currentGame = null;
        PublishGameEnd();
    }
}
