using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    [Header("敵1体の得点")]
    [SerializeField] private int _oneEnemyScore = 100;
    [Header("倍率")]
    [SerializeField] private List<int> _scoreMagnification = new List<int>();

    [Header("マイナス得点")]
    [SerializeField] private int _minuss = 5000;
    [Header("マイナスのUI")]
    [SerializeField] private List<GameObject> _minussScoreUIWorld = new List<GameObject>();

    [SerializeField] private Transform _playerPos;

    [Header("===UIをCunvus上に出すかどうか===")]
    [SerializeField] private bool _isCunvus = true;
    [Header("ScoreのUI_Canvus")]
    [SerializeField] private List<GameObject> _scoreUICunvus = new List<GameObject>();
    [Header("ScoreのUI_World")]
    [SerializeField] private List<GameObject> _scoreUIWorld = new List<GameObject>();

    [Header("Offset")]
    [SerializeField] private Vector3 _offSet;

    /// <summary>スコアを獲得</summary>
    /// <param name="i">連続して倒した敵の数</param>
    public void AddScore(int i, Transform position)
    {
        if (i >= _scoreMagnification.Count)
        {
            i = _scoreMagnification.Count - 1;
        }

        int addScore = _oneEnemyScore * _scoreMagnification[i];

        if (_isCunvus)
        {
            UIOnCanvus(position);
        }
        else
        {
            UIOnWorld(position,addScore);
        }

        GameManager.I.AddScore(addScore);
    }

    public void DissScore()
    {
        GameManager.I.AddScore(_minuss);

        for (int i = 0; i < _minussScoreUIWorld.Count; i++)
        {
            if (_minussScoreUIWorld[i].activeSelf == false)
            {
                _minussScoreUIWorld[i].SetActive(true);
                _minussScoreUIWorld[i].transform.GetChild(3).gameObject.GetComponent<Text>().text =_minuss.ToString();
                // **UIの位置をスクリーン座標に設定**
                _minussScoreUIWorld[i].transform.position = _playerPos.position;
                return;
            }
        }
    }

   
    public void UIOnCanvus(Transform enemyPos)
    {
        // **ワールド座標をスクリーン座標に変換**
        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemyPos.position);

        for (int i = 0; i < _scoreUICunvus.Count; i++)
        {
            if (_scoreUICunvus[i].activeSelf == false)
            {
                _scoreUICunvus[i].gameObject.SetActive(true);
                // **UIの位置をスクリーン座標に設定**
                _scoreUICunvus[i].transform.position = screenPos;
                return;
            }
        }
    }

    public void UIOnWorld(Transform enemyPos,int score)
    {
        for (int i = 0; i < _scoreUIWorld.Count; i++)
        {
            if (_scoreUIWorld[i].activeSelf == false)
            {
                _scoreUIWorld[i].SetActive(true);
                _scoreUIWorld[i].transform.GetChild(1).gameObject.GetComponent<Text>().text = score.ToString();
                // **UIの位置をスクリーン座標に設定**
                _scoreUIWorld[i].transform.position = enemyPos.position + _offSet;
                return;
            }
        }
    }


}
