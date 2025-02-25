using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class PlayerAttack
{
    [Header("攻撃のため時間")]
    [SerializeField] private float _attackChageTime = 1;

    [Header("攻撃のクールダウン")]
    [SerializeField] private float _coolTime = 0.3f;

    [Header("エフェクト")]
    [SerializeField] private List<ParticleSystem> _chargeEffect = new List<ParticleSystem>();

    [Header("弾を出す位置")]
    [SerializeField] private Transform _muzzlePos;

    [Header("弾")]
    [SerializeField] private GameObject _bullet;

    private float _countChargeTime = 0;

    private float _countCoolTime = 0;

    private bool _isCharge = false;

    private bool _isCoolTime = true;

    private bool _isChangeCamera = false;

    private bool _isReleaseAttackButtun = false;

    public bool IsCharge => _isCharge;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _chargeEffect.ForEach(i => i.Stop());
    }

    public void Charge()
    {
        if (!_isCoolTime)
        {
            _countCoolTime += Time.deltaTime;

            if (_countCoolTime > _coolTime)
            {
                _isCoolTime = true;
            }
            return;
        }

        if (_playerControl.InputM.IsLeftMouseClickUp)
        {
            _isReleaseAttackButtun = true;
        }   //攻撃ボタンを離したかどうか


        if (_playerControl.InputM.IsLeftMouseClickDown)
        {
            _isCharge = true;

            _playerControl.CameraSetting.ChangeCameraPriority(CameraType.Attack);
            _isChangeCamera = true;

            if (_chargeEffect.Count > 0)
            {
                _chargeEffect.ForEach(i => i.Play());
            }
        }   //攻撃ボタンを押したかどうか

        if (_isCharge)
        {
            _countChargeTime += Time.deltaTime;
        }


        if (_countChargeTime > _attackChageTime && _isReleaseAttackButtun)
        {
            Attack();
        }
    }

    public void Attack()
    {
        _playerControl.CameraSetting.ShakeCamera(CameraType.Attack);

        _isCharge = false;
        _isChangeCamera = false;
        _isCoolTime = false;
        _isReleaseAttackButtun = false;
        _countCoolTime = 0;
        _countChargeTime = 0;

        _chargeEffect.ForEach(i => i.Stop());

        _playerControl.CameraSetting.ResetChangeCameraCount();


        var go = GameObject.Instantiate(_bullet);
        go.transform.position = _muzzlePos.position;
        Vector3 dir = _muzzlePos.position - _playerControl.transform.position;
        go?.GetComponent<PlayerBullet>()?.Init(dir);
    }

}
