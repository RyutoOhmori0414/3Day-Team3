using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class IdleState : PlayerStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        var h = _stateMachine.PlayerControl.InputM.HorizontalInput;
        var v = _stateMachine.PlayerControl.InputM.VerticalInput;

        if (h != 0 || v != 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWalk);
            return;
        }
    }
}
