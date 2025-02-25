using UnityEngine;

public class Enemy_B : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed;
    protected GameObject _player;
    private void Start()
    {
        Start_S();
        _player = GameObject.FindGameObjectWithTag("Player");//FindAnyObjectByType‚Å‚â‚è‚½‚¢‚ªPlayer‚ª‚Ü‚¾‚È‚¢‚Ì‚Å‚±‚¤‚µ‚Ä‚¢‚é
    }

    private void Update()
    {
        Update_S();
    }

    private void FixedUpdate()
    {
        var dir = _player.transform.position - transform.position;
        if (dir.magnitude >= 0.2)
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
        }
    }


    protected virtual void Start_S() { }
    protected virtual void Update_S() { }
}
