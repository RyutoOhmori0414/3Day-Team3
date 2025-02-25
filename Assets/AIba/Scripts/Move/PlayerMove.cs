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

            _playerControl.Rb.linearVelocity = new Vector3(-_speedLimit.x, _playerControl.Rb.linearVelocity.y, _playerControl.Rb.linearVelocity.z);
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
