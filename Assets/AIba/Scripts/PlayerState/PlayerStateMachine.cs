using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerStateMachine : StateMachine
{

    #region State
    [SerializeField]
    private IdleState _stateIdle = default;
    [SerializeField]
    private WalkState _stateWalk = default;



    private PlayerControl _playerController = null;

    public PlayerControl PlayerControl => _playerController;

    public IdleState StateIdle => _stateIdle;
    public WalkState StateWalk => _stateWalk;

    #endregion
    [SerializeField]
    //private GroundCheck _groundCheck;
    //public GroundCheck GroundCheck => _groundCheck;

    public void Init(PlayerControl playerController)
    {
        _playerController = playerController;
        Initialize(_stateIdle);
    }

    protected override void StateInit()
    {
        _stateIdle.Init(this);
        _stateWalk.Init(this);
    }



}
