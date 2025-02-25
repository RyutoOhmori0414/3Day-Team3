using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour,IDamageble
{
    [Header("移動設定")]
    [SerializeField] private PlayerMove _move;

    [Header("攻撃設定")]
    [SerializeField] private PlayerAttack _attack;

    [Header("体力")]
    [SerializeField] private PlayerHp _hp;

    [Header("カメラ設定")]
    [SerializeField] private PlayerCamera _cameraSetting;


    [Header("PlayerのRigidbody")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private InputManager _input;
    [SerializeField] private PlayerStateMachine _stateMachine = default;

    public PlayerMove Move => _move;
    public PlayerCamera CameraSetting => _cameraSetting;
    public PlayerAttack Attack => _attack;
    public PlayerHp Hp => _hp;

    public Rigidbody Rb => _rb;
    public InputManager InputM => _input;

    void Start()
    {
        _stateMachine.Init(this);
        _move.Init(this);
        _cameraSetting.Init(this);
        _attack.Init(this);
        _hp.Init(this);
    }


    void Update()
    {
        _stateMachine.Update();
        _attack.Charge();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();

        // Debug.Log(_rb.linearVelocity);
    }

    private void LateUpdate()
    {
        _stateMachine.LateUpdate();
    }

    public void AddDamage(int damagePoint)
    {
        _hp.Damage(damagePoint);
    }
}
