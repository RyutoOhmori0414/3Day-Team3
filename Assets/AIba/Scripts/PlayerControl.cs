using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private PlayerMove _move;

    [Header("PlayerのRigidbody")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private InputManager _input;
    [SerializeField] private PlayerStateMachine _stateMachine = default;

    public PlayerMove Move => _move;

    public Rigidbody Rb => _rb;
    public InputManager InputM => _input;

    void Start()
    {
        _stateMachine.Init(this);
        _move.Init(this);
    }


    void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        _stateMachine.LateUpdate();
    }

}
