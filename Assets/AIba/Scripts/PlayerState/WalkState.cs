using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerControl.CameraSetting.ResetChangeCameraCount();

        //アニメーション設定
        _stateMachine.PlayerControl.Anim.Move(true);
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerControl.Move.Move();
        _stateMachine.PlayerControl.Move.SpeedLimit();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        var h = _stateMachine.PlayerControl.InputM.HorizontalInput;
        var v = _stateMachine.PlayerControl.InputM.VerticalInput;

        //カメラの設定
        //_stateMachine.PlayerControl.CameraSetting.MoveCameraFOV(true);



        if (h != 0 || v != 0)
        {
            _stateMachine.PlayerControl.CameraSetting.ChangeCamera(CameraType.Move, true);
        }
        else
        {
            _stateMachine.PlayerControl.CameraSetting.ChangeCamera(CameraType.Move, false);
        }

        if (h == 0 && v == 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }
    }
}
