using UnityEngine;

public class Enemy_B : MonoBehaviour
{
    [SerializeField,Header("スピード")] protected float _moveSpeed = 1;
    [SerializeField, Header("止まる距離")] float _stopDistance = 0.2f;
    protected GameObject _player;
    private void Start()
    {
        Start_S();
        _player = GameObject.FindGameObjectWithTag("Player");//FindAnyObjectByTypeでやりたいがPlayerがまだないのでこうしている
    }

    private void Update()
    {
        Update_S();
    }

    private void FixedUpdate()
    {
        var dir = _player.transform.position - transform.position;
        if (dir.magnitude >= _stopDistance)
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")//Tagでやりたくはない
        {
            //ダメージ処理
        }
    }

    protected virtual void Start_S() { }
    protected virtual void Update_S() { }
}
