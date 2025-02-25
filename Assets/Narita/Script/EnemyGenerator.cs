using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Header("スポーンする場所")]
    List<Transform> _spawnPosList;
    [SerializeField, Header("インターバル")]
    float _spawnInterval = 5;
    [SerializeField, Header("インターバルの減少量")]
    float _decreaseToInterval = 0.5f;
    [SerializeField, Header("最短インターバル")]
    float _minInterval = 1;
    [SerializeField, Header("敵のリスト")]
    List<GameObject> _enemiesList;
    float _timer;

    void Start()
    {

    }

    void Update()
    {
        if (_enemiesList.Count != 0 && _spawnPosList.Count != 0)
        {
            if (Time.time >= _timer + _spawnInterval)
            {
                //ランダムな敵をランダムな場所に生成
                _timer = Time.time;
                var randomSpawn = Random.Range(0, _spawnPosList.Count);
                var randomEnemy = Random.Range(0, _enemiesList.Count);
                Instantiate(_enemiesList[randomEnemy], _spawnPosList[randomSpawn].position, Quaternion.identity);
                if (_spawnInterval > _minInterval)//インターバルの減少
                {
                    _spawnInterval -= _decreaseToInterval;
                    if (_spawnInterval < _minInterval)//最小を下回るときに最小に固定
                    {
                        _spawnInterval = _minInterval;
                    }
                }
            }
        }
    }
}
