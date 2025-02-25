using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("速度")]
    [SerializeField] private float _speed = 7;

    [Header("威力")]
    [SerializeField] private int _attackPower = 7;

    [Header("消えるまでの時間")]
    [SerializeField] private float _lifeTime = 10f;


    private float _countDestroyTime = 0;

    public void Init(Vector3 dir)
    {
        gameObject.GetComponent<Rigidbody>().linearVelocity = dir.normalized * _speed;
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
        if (other.gameObject.tag == "Enemy")
        {
            other?.GetComponent<IDamageble>()?.AddDamage(_attackPower);
        }

        Destroy(gameObject);
    }

}
