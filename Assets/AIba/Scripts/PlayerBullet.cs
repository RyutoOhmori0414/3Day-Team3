using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBullet : MonoBehaviour
{
    [Header("弾のタイプ")]
    [SerializeField] private BulletType _bulletType;

    [Header("速度")]
    [SerializeField] private float _speed = 7;

    [Header("威力")]
    [SerializeField] private int _attackPower = 7;

    [Header("消えるまでの時間")]
    [SerializeField] private float _lifeTime = 10f;

    [Header("反射回数")]
    [SerializeField] private int _hitNum = 4;

    [Header("反射角度")]
    private float randomAngleRange = 30f; // 直撃時のランダム反射範囲


    private int _hitCount = 0;

    private float _countDestroyTime = 0;

    private Vector3 _nowDir = default;

    private ScoreManager _scoreManager;

    public void Init(Vector3 dir,ScoreManager scoreManager)
    {
        _nowDir = dir;
        gameObject.GetComponent<Rigidbody>().linearVelocity = dir.normalized * _speed;

        _scoreManager = scoreManager;
    }


    void Update()
    {
        _countDestroyTime += Time.deltaTime;

        if (_countDestroyTime > _lifeTime)
        {
            Destroy(gameObject);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Move")
        {
            return;
        }

        _hitCount++;

        if (other.gameObject.tag == "Enemy")
        {
            other?.GetComponent<IDamageble>()?.AddDamage(_attackPower);
            _scoreManager.AddScore(_hitCount, other.gameObject.transform);
        }

        if (_bulletType == BulletType.Penetration)
        {
            if (other.gameObject.tag != "Enemy")
            {
                Destroy(gameObject);
            }
            else
            {
                if (_hitCount == _hitNum)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (_bulletType == BulletType.Reflection)
        {
            Vector3 normal = other.ClosestPoint(transform.position) - transform.position;
            normal = normal.normalized; // 法線を正規化

            // **通常の反射計算**
            Vector3 reflectDir = Vector3.Reflect(_nowDir, normal);

            // **直角に当たった場合はランダムに角度をずらす**
            if (Vector3.Dot(_nowDir, normal) < -0.9f) // 直角に近い場合
            {
                reflectDir = RandomizeDirection(reflectDir, randomAngleRange);
            }

            // **新しい方向に更新**
            _nowDir = reflectDir;
            _nowDir.y = 0;

            gameObject.GetComponent<Rigidbody>().linearVelocity = _nowDir * _speed;

            if (_hitCount == _hitNum)
            {
                Destroy(gameObject);
            }
        }
    }

    // ランダムな角度で方向をずらす
    Vector3 RandomizeDirection(Vector3 direction, float angleRange)
    {
        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(-angleRange, angleRange), // X軸方向のランダム回転
            Random.Range(-angleRange, angleRange), // Y軸方向のランダム回転
            Random.Range(-angleRange, angleRange)  // Z軸方向のランダム回転
        );
        return randomRotation * direction;
    }

}

public enum BulletType
{
    /// <summary>反射 </summary>
    Reflection,

    /// <summary>貫通</summary>
    Penetration,

}