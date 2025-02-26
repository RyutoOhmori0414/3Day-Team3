using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private PlayerMove _move;

    [Header("攻撃設定")]
    [SerializeField] private PlayerAttack _attack;

    [Header("体力")]
    [SerializeField] private PlayerHp _hp;

    [Header("カメラ設定")]
    [SerializeField] private PlayerCamera _cameraSetting;

    [Header("Animation設定")]
    [SerializeField] private PlayerAnim _anim;

    [Header("Effect設定")]
    [SerializeField] private PlayerEffect _effect;

    [Header("PlayerのRigidbody")]
    [SerializeField] private Rigidbody _rb;

    [Header("プレイヤーのイメージ位置")]
    [SerializeField] private Transform _playerImage;

    [SerializeField] private InputManager _input;
    [SerializeField] private PlayerStateMachine _stateMachine = default;

    public PlayerMove Move => _move;
    public PlayerCamera CameraSetting => _cameraSetting;
    public PlayerAttack Attack => _attack;
    public PlayerHp Hp => _hp;
    public PlayerAnim Anim => _anim;
    public PlayerEffect Effect => _effect;

    public Transform PlayerImage => _playerImage;
    public Rigidbody Rb => _rb;
    public InputManager InputM => _input;

    void Start()
    {
        GameManager.I.PlayReady();

        _stateMachine.Init(this);
        _move.Init(this);
        _cameraSetting.Init(this);
        _attack.Init(this);
        _hp.Init(this);
        _anim.Init(this);
        _effect.Init(this);
    }


    void Update()
    {
        _stateMachine.Update();
        _attack.Charge();
        _attack.ChangeBulletType();

        _anim.AnimUpdata();
        _effect.EffectUpdata();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();

        // Debug.Log(_rb.linearVelocity);
    }

    private void LateUpdate()
    {
        _stateMachine.LateUpdate();
        _attack.AttackLineRender();
    }
}
