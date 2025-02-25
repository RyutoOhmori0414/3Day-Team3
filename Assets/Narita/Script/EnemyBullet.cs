using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField, Header("与ダメ")]
    int _damage;
    [SerializeField, Header("弾速")]
    float _speed = 2;
    Vector3 _dir;
    [SerializeField, Header("生成してから弾を破壊するまでの時間")]
    float _destroyTime;
    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }
    void FixedUpdate()
    {
        transform.position += _dir.normalized * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.TryGetComponent(out IDamageble component))
            {
                component.AddDamage(_damage);
            }
        }
        Destroy(gameObject);
    }
    public void AddTarget(GameObject player)
    {
        _dir = player.transform.position - transform.position;
    }
}
