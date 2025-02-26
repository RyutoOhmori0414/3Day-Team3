using TMPro.EditorUtilities;
using UnityEngine;

public abstract class Enemy_B : MonoBehaviour, IDamageble
{
    [SerializeField, Header("スピード")]
    protected float _moveSpeed = 1;
    [SerializeField, Header("止まる距離")]
    float _stopDistance = 0.2f;
    [SerializeField, Header("ぶつかったときのダメージ")]
    int _hitDamage = 1;
    protected GameObject _player;
    [SerializeField] int _life = 5;
    [SerializeField] GameObject _deathEffect;
    [SerializeField] Animator _anim;
    private void Start()
    {
        FindAnyObjectByType<GameManager>().DefeatEnemy();
        Start_S();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Update_S();
        if (_player == null) return;
        var dir = _player.transform.position - transform.position;

        if (dir.magnitude >= _stopDistance)
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
        }
        else if (dir.magnitude < _stopDistance - 0.1f)
        {
            transform.position += -dir.normalized * _moveSpeed * Time.deltaTime;
        }

        if (dir.x > 0)
        {
            _anim.SetTrigger("TurnRight");
        }
        if (dir.x < 0)
        {
            _anim.SetTrigger("TurnLeft");
        }

        transform.forward = Camera.main.transform.forward;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.TryGetComponent(out IDamageble component))
            {
                component.AddDamage(_hitDamage);
            }
        }
    }

    protected virtual void Start_S() { }
    protected virtual void Update_S() { }
    bool IDamageble.AddDamage(int damagePoint)
    {
        _life -= damagePoint;
        if (_life <= 0)
        {
            var effect = Instantiate(_deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1);
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
