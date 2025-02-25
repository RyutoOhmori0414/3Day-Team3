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

    [Header("グループカメラのターゲット")]
    [SerializeField] private GameObject _groupTarget;

    /// <summary>カメラ推移までの待機時間を計測</summary>
    private float _countChangeWaitTime = 0;

    /// <summary>カメラの切り替えをしたかどうか</summary>
    private bool _isChangeCamera = false;

    private bool _isNoChange = false;
    private PlayerControl _playerControl;

   [SerializeField] private CinemachineImpulseSource _attackImpulse;


    private CameraType _cameraType = CameraType.Idle;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    void Start()
    {
        //_follow.FollowOffset = new Vector3(2, 4.1f, -6.55f);
    }

    // Update is called once per frame
    public void CameraGroupSetting()
    {
        // **マウスのスクリーン座標を取得**
        Vector3 mouseScreenPos = Input.mousePosition;

        // **画面の幅と高さを取得**
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // **マウスの座標を 0.0 〜 1.0 に正規化**
        float normalizedX = Mathf.Clamp01(mouseScreenPos.x / screenWidth);
        float normalizedY = Mathf.Clamp01(mouseScreenPos.y / screenHeight);

        // **マウス位置を基準にY軸の角度を計算**
        float targetAngle = Mathf.Atan2(normalizedX - 0.5f, normalizedY - 0.5f) * Mathf.Rad2Deg;

        // **Y軸のみ回転**
        _groupTarget.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
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

    public void ShakeCamera(CameraType cameraType)
    {
        if(cameraType == CameraType.Attack)
        {
            _attackImpulse.GenerateImpulse();
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