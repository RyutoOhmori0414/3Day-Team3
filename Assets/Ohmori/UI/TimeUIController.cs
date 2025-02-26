using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class TimeUIController : MonoBehaviour, IGameStartReciever, IGameEndReciever
{
    [SerializeField]
    private Text _timeText;

    private bool _isIngame = false;

    private void Awake()
    {
        _timeText.text = "00:00";
    }

    private void Update()
    {
        if (!_isIngame) return;
        
        _timeText.text = $"{Mathf.Max(Mathf.FloorToInt(GameManager.I.CurrentGameElapsedSeconds / 60), 0):00}:{(GameManager.I.CurrentGameElapsedSeconds % 60):00}";
    }

    public void GameStart()
    {
        _isIngame = true;
    }

    public void GameEnd()
    {
        _isIngame = false;
    }
}
