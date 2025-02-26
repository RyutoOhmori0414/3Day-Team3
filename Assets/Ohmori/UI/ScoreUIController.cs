using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIController : MonoBehaviour, IGameStartReciever, IGameEndReciever
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private float _duration = 1.0F;

    private Tweener _tweener;
    
    private void UpdateScore(int score)
    {
        _tweener?.Kill();
        
        _tweener = _scoreText.DOText(score.ToString("00000"), _duration, true, ScrambleMode.Numerals);
    }

    public void GameStart()
    {
        GameManager.I.OnScoreChanged += UpdateScore;
    }

    public void GameEnd()
    {
        GameManager.I.OnScoreChanged -= UpdateScore;
    }
}
