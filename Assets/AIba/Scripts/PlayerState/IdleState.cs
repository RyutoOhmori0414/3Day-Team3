using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class IdleState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerControl.CameraSetting.ResetChangeCameraCount();

        //アニメーション設定
        _stateMachine.PlayerControl.Anim.Move(false);
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

        //カメラ設定
        //  _stateMachine.PlayerControl.CameraSetting.MoveCameraFOV(false);
        if (h == 0 && v == 0)
        {
            _stateMachine.PlayerControl.CameraSetting.ChangeCamera(CameraType.Idle, true);
        }
        else
        {
            _stateMachine.PlayerControl.CameraSetting.ChangeCamera(CameraType.Idle, false);
        }

        _stateMachine.PlayerControl.CameraSetting.CameraGroupSetting();

        if (h != 0 || v != 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWalk);
            return;
        }
    }
}
