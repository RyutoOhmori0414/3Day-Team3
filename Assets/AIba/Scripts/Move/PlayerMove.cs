using UnityEngine;

[System.Serializable]
public class PlayerMove
{
    [Header("移動速度")]
    [SerializeField] private float _moveSpeed = 3;

    [Header("速度制限")]
    [SerializeField] private Vector3 _speedLimit = new Vector3(5, 2, 5);

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

        _playerControl.Rb.AddForce(moveDir.normalized * _moveSpeed);
    }

    public void SpeedLimit()
    {
        //X
        if (_playerControl.Rb.angularVelocity.x > _speedLimit.x)
        {
            _playerControl.Rb.angularVelocity = new Vector3(_speedLimit.x, _playerControl.Rb.angularVelocity.y, _playerControl.Rb.angularVelocity.z);
        }
        else if (_playerControl.Rb.angularVelocity.x < -_speedLimit.x)
        {
            _playerControl.Rb.angularVelocity = new Vector3(-_speedLimit.x, _playerControl.Rb.angularVelocity.y, _playerControl.Rb.angularVelocity.z);
        }

        //Y
        if (_playerControl.Rb.angularVelocity.y > _speedLimit.y)
        {
            _playerControl.Rb.angularVelocity = new Vector3(_playerControl.Rb.angularVelocity.x, _speedLimit.x, _playerControl.Rb.angularVelocity.z);
        }
        else if (_playerControl.Rb.angularVelocity.y < -_speedLimit.y)
        {
            _playerControl.Rb.angularVelocity = new Vector3(_playerControl.Rb.angularVelocity.x, -_speedLimit.x, _playerControl.Rb.angularVelocity.z);
        }


        //Z
        if (_playerControl.Rb.angularVelocity.y > _speedLimit.z)
        {
            _playerControl.Rb.angularVelocity = new Vector3(_playerControl.Rb.angularVelocity.x, _playerControl.Rb.angularVelocity.y, _speedLimit.z);
        }
        else if (_playerControl.Rb.angularVelocity.y < -_speedLimit.z)
        {
            _playerControl.Rb.angularVelocity = new Vector3(_playerControl.Rb.angularVelocity.x, _playerControl.Rb.angularVelocity.y, -_speedLimit.z);
        }
    }


}
