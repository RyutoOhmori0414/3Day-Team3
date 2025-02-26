using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class PlayerCamera
{

    [Header("カメラ推移までの待機時間")]
    [SerializeField] private float _cameraChangeWaitTime = 2;

    [Header("プレイヤーの通常のカメラ")]
    [SerializeField] private CinemachineCamera _defultPlayerCamera;
    [Header("プレイヤーの移動のカメラ")]
    [SerializeField] private CinemachineCamera _movePlayerCamera;
    [SerializeField] private CinemachineFollow _follow;

    [Header("プレイヤーの攻撃のカメラ")]
    [SerializeField] private CinemachineCamera _attackCamera;

    [Header("止まってる時の通常視野角")]
    [SerializeField] float _defultFOV = 60;
    [Header("移動時の最大視野角")]
    [SerializeField] float _maxFOV = 70;
    [Header("視野角の変更速度_増やす")]
    [SerializeField] float _FOVChangeSpeed = 0.3f;
    [Header("視野角の変更速度_減らす")]
    [SerializeField] float _FOVChangeSpeedRemove = 0.6f;


    [Header("プレイヤーの中心")]
    [SerializeField] private GameObject _playerCenter;
    [Header("グループカメラのターゲット")]
    [SerializeField] private GameObject _groupTarget;

    /// <summary>カメラ推移までの待機時間を計測</summary>
    private float _countChangeWaitTime = 0;

    /// <summary>カメラの切り替えをしたかどうか</summary>
    private bool _isChangeCamera = false;

    private bool _isNoChange = false;
    private PlayerControl _playerControl;


    [Header("振動_小")]
    [SerializeField] private CinemachineImpulseSource _attackImpulseSmaal;
    [Header("振動_中")]
    [SerializeField] private CinemachineImpulseSource _attackImpulseMidium;
    [Header("振動_大")]
    [SerializeField] private CinemachineImpulseSource _attackImpulseBig;

    private CameraType _cameraType = CameraType.Idle;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    void Start()
    {
        //_follow.FollowOffset = new Vector3(2, 4.1f, -6.55f);
    }

    public void ResetChangeCameraCount()
    {
        _isChangeCamera = false;
        _countChangeWaitTime = 0;
    }

    public void ChangeCamera(CameraType cameraType, bool isInput)
    {
        if (_isChangeCamera || _playerControl.Attack.IsCharge) return;

        if (isInput)
        {
            // Debug.Log(_countChangeWaitTime);
            _countChangeWaitTime += Time.deltaTime;

            if (_countChangeWaitTime > _cameraChangeWaitTime)
            {
                ChangeCameraPriority(cameraType);
                _isChangeCamera = true;
            }
        }
        else
        {
            _countChangeWaitTime = 0;
        }
    }

    public void ShakeCamera(CameraShakeType cameraType)
    {
        if (cameraType == CameraShakeType.AttackMin)
        {
            _attackImpulseSmaal.GenerateImpulse(1);
        }
        else if (cameraType == CameraShakeType.AttackMidium)
        {
            _attackImpulseSmaal.GenerateImpulse(2);
        }
        else if (cameraType == CameraShakeType.AttackBig)
        {
            _attackImpulseSmaal.GenerateImpulse(3);
        }
    }


    public void ChangeCameraPriority(CameraType cameraType)
    {
        _defultPlayerCamera.Priority = 0;
        _movePlayerCamera.Priority = 0;
        _attackCamera.Priority = 0;

        if (cameraType == CameraType.Idle)
        {
            _defultPlayerCamera.Priority = 1;
        }
        else if (cameraType == CameraType.Move)
        {
            _movePlayerCamera.Priority = 1;
        }
        else if (cameraType == CameraType.Attack)
        {
            _attackCamera.Priority = 1;
        }

    }


    /// <summary>移動している時のカメラの設定。視野の調整をする</summary>
    public void MoveCameraFOV(bool isMove)
    {
        if (_isNoChange) return;

        if (isMove)
        {
            if (_defultPlayerCamera.Lens.FieldOfView < _maxFOV)
            {
                _defultPlayerCamera.Lens.FieldOfView += _FOVChangeSpeed;
            }
        }
        else
        {
            if (_defultPlayerCamera.Lens.FieldOfView > _defultFOV)
            {
                _defultPlayerCamera.Lens.FieldOfView -= _FOVChangeSpeedRemove;
            }
        }

    }


}

public enum CameraType
{
    Idle,
    Move,
    Attack,
}

public enum CameraShakeType
{
    AttackMin,
    AttackMidium,
    AttackBig,
}