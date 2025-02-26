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

    [Header("レイヤー")]
    [SerializeField] private LayerMask _layer;
    [Header("長さ")]
    [SerializeField] private float _rayLong = 20;
    [Header("Y軸の高さ")]
    [SerializeField] private float _setYPos;
    [Header("LineRender")]
    [SerializeField] private LineRenderer _lr;



    [Header("弾を出す位置")]
    [SerializeField] private Transform _muzzlePos;

    [Header("反射の弾")]
    [SerializeField] private GameObject _reflectionbullet;
    [Header("貫通弾")]
    [SerializeField] private GameObject _penetrationBullet;

    [Header("反射のUI")]
    [SerializeField] private GameObject _reflectionImage;
    [Header("貫通のUI")]
    [SerializeField] private GameObject _penetrationImage;

    [SerializeField] private ScoreManager _scoreManager;

    private BulletType _bulletType = BulletType.Penetration;

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

    public void ChangeBulletType()
    {
        if (_isCharge) return;

        if (_playerControl.InputM.IsRightMouseClickDown)
        {
            if (_bulletType == BulletType.Reflection)
            {
                _bulletType = BulletType.Penetration;
                _penetrationImage.SetActive(true);
                _reflectionImage.SetActive(false);
            }
            else
            {
                _bulletType = BulletType.Reflection;
                _penetrationImage.SetActive(false);
                _reflectionImage.SetActive(true);
            }
        }
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

    public void AttackLineRender()
    {
        _lr.SetPosition(0, _muzzlePos.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 worldPos = default;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
        {
            worldPos = hit.point;
            worldPos = new Vector3(worldPos.x, _setYPos, worldPos.z);
            _lr.SetPosition(1, worldPos);
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

        GameObject go;
        if (_bulletType == BulletType.Reflection)
        {
            go = GameObject.Instantiate(_reflectionbullet);
        }
        else
        {
            go = GameObject.Instantiate(_penetrationBullet);
        }

        go.transform.position = _muzzlePos.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 worldPos = default;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
        {
            worldPos = hit.point;
            worldPos = new Vector3(worldPos.x, _setYPos, worldPos.z);
        }

        Vector3 dir = worldPos - _muzzlePos.position;
        dir.y =0;
        go?.GetComponent<PlayerBullet>()?.Init(dir,_scoreManager);
    }

}
