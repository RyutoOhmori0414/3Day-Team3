using UnityEngine;

[System.Serializable]
public class PlayerMove
{
    [Header("移動速度")]
    [SerializeField] private float _moveSpeed = 3;
    [Header("速度制限")]
    [SerializeField] private Vector3 _speedLimit = new Vector3(5, 2, 5);

    [Header("移動速度の軽減速度")]
    private float _ressRbSpeed = 0.05f;

    [Header("ダッシュ")]
    [SerializeField] private float _dashPower = 6f;
    [Header("ダッシュの実行時間")]
    [SerializeField] private float _dashTime = 0.5f;
    [Header("ダッシュのクールタイム")]
    [SerializeField] private float _dashCoolTime = 4f;

    private bool _isDoDah = false;
    private bool _isCanDash = true;
    private float _countDashCoolTime = 0;
    private float _countDashTime = 0;


    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Move()
    {
        var h = _playerControl.InputM.HorizontalInput;
        var v = _playerControl.InputM.VerticalInput;

        Vector3 moveDir = new Vector3(h, 0, v);

        Vector3 saveSpeed = _playerControl.Rb.linearVelocity;

        //速度超過している場合は、速度を加えない_X速度
        if (saveSpeed.x > _speedLimit.x && h > 0)
        {
            moveDir.x = 0;
        }
        else if (saveSpeed.x < -_speedLimit.x && h < 0)
        {
            moveDir.x = 0;
        }

        //速度超過している場合は、速度を加えない_X速度
        if (saveSpeed.z > _speedLimit.z && v > 0)
        {
            moveDir.z = 0;
        }
        else if (saveSpeed.z < -_speedLimit.z && v < 0)
        {
            moveDir.z = 0;
        }
        _playerControl.Rb.AddForce(moveDir.normalized * _moveSpeed);
    }

    public void Dash()
    {
        if (!_isCanDash)
        {
            _countDashCoolTime += Time.deltaTime;

            if (_countDashCoolTime > _dashCoolTime)
            {
                _isCanDash = true;
                _countDashCoolTime = 0;
            }
            return;
        }   //クールタイム
        else if (_isDoDah)
        {
            var h = _playerControl.InputM.HorizontalInput;
            var v = _playerControl.InputM.VerticalInput;

            if(h==0 && v==0)
            {
                _isCanDash = false;
                _isDoDah = false;
                _countDashTime = 0;
                _playerControl.Effect.Dash.ForEach(i => i.SetActive(false));
                _playerControl.Effect.DashStop.SetActive(true);
            }


            _countDashTime += Time.deltaTime;

            if(_countDashTime>_dashTime)
            {
                _isCanDash = false;
                _isDoDah = false;
                _countDashTime = 0;
            }
            if (_playerControl.Attack.IsDoMoveAttack)
            {
                _playerControl.Rb.linearVelocity = Vector3.zero;
            }

            Vector3 moveDir = new Vector3(h, 0, v);
            _playerControl.Rb.linearVelocity = moveDir.normalized * _dashPower;
        }


        //方向転換中は不可


        if (_playerControl.InputM.IsLeftShiftDown)
        {
            _isDoDah = true;
            _playerControl.Effect.Dash.ForEach(i => i.SetActive(true));
        }

    }

    /// <summary>
    /// 速度の調整
    /// </summary>
    public void SpeedLimit()
    {
        //X
        if (_playerControl.Rb.linearVelocity.x > _speedLimit.x)
        {
            float setX = _playerControl.Rb.linearVelocity.x - _ressRbSpeed;

            _playerControl.Rb.linearVelocity = new Vector3(setX, _playerControl.Rb.linearVelocity.y, _playerControl.Rb.linearVelocity.z);
        }
        else if (_playerControl.Rb.linearVelocity.x < -_speedLimit.x)
        {
            float setX = _playerControl.Rb.linearVelocity.x + _ressRbSpeed;

            _playerControl.Rb.linearVelocity = new Vector3(setX, _playerControl.Rb.linearVelocity.y, _playerControl.Rb.linearVelocity.z);
        }

        //Y
        if (_playerControl.Rb.linearVelocity.y > _speedLimit.y)
        {
            float setY = _playerControl.Rb.angularVelocity.y - _ressRbSpeed;

            _playerControl.Rb.linearVelocity = new Vector3(_playerControl.Rb.linearVelocity.x, setY, _playerControl.Rb.linearVelocity.z);
        }
        else if (_playerControl.Rb.linearVelocity.y < -_speedLimit.y)
        {
            float setY = _playerControl.Rb.angularVelocity.y + _ressRbSpeed;

            _playerControl.Rb.angularVelocity = new Vector3(_playerControl.Rb.linearVelocity.x, setY, _playerControl.Rb.linearVelocity.z);
        }


        //Z
        if (_playerControl.Rb.linearVelocity.y > _speedLimit.z)
        {
            float setZ = _playerControl.Rb.linearVelocity.z - _ressRbSpeed;

            _playerControl.Rb.angularVelocity = new Vector3(_playerControl.Rb.linearVelocity.x, _playerControl.Rb.linearVelocity.y, setZ);
        }
        else if (_playerControl.Rb.angularVelocity.y < -_speedLimit.z)
        {
            float setZ = _playerControl.Rb.linearVelocity.y + _ressRbSpeed;
            _playerControl.Rb.angularVelocity = new Vector3(_playerControl.Rb.linearVelocity.x, _playerControl.Rb.linearVelocity.y, setZ);
        }
    }


}
