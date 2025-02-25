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
    int _life;
    private void Start()
    {
        FindAnyObjectByType<GameManager>().DefeatEnemy();
        Start_S();
        _player = GameObject.FindGameObjectWithTag("Player");//FindAnyObjectByTypeでやりたいがPlayerがまだないのでこうしている
    }

    private void Update()
    {
        Update_S();
    }

    private void FixedUpdate()
    {
        Debug.Assert(_player != null);
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

        dir.y = 0;
        var cross = Vector3.Cross(transform.forward, dir);
        if (cross.y < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (cross.y > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            return;
        }
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

    public void AddDamage(int damagePoint)
    {
        _life -= damagePoint;
    }
}
