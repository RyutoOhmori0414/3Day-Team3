using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    [Header("敵1体の得点")]
    [SerializeField] private int _oneEnemyScore = 100;

    [Header("倍率")]
    [SerializeField] private List<int> _scoreMagnification = new List<int>();

    
    
    /// <summary>スコアを獲得</summary>
    /// <param name="i">連続して倒した敵の数</param>
    public void AddScore(int i)
    {


    }

}
