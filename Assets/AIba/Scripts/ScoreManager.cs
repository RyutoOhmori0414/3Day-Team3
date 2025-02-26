using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    [Header("敵1体の得点")]
    [SerializeField] private int _oneEnemyScore = 100;
    [Header("倍率")]
    [SerializeField] private List<int> _scoreMagnification = new List<int>();

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
            UIOnWorld(position);
        }

        GameManager.I.AddScore(addScore);
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

    public void UIOnWorld(Transform enemyPos)
    {
        for (int i = 0; i < _scoreUIWorld.Count; i++)
        {
            if (_scoreUIWorld[i].activeSelf == false)
            {
                _scoreUIWorld[i].SetActive(true);
                // **UIの位置をスクリーン座標に設定**
                _scoreUIWorld[i].transform.position = enemyPos.position + _offSet;
                return;
            }
        }
    }


}
