using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] int _damage;
    [SerializeField] float _speed = 2;
    GameObject _player;
    Vector3 _dir;
    void Start()
    {
        _dir = _player.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        transform.position += _dir.normalized * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageble component))
        {
            component.AddDamage(_damage);
        }
        Destroy(gameObject);
    }
    public void AddTarget(GameObject player)
    {
        _player = player;
    }
}
