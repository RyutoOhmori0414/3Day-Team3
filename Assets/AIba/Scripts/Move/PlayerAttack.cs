using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

[System.Serializable]
public class PlayerAttack
{
    [Header("溜めの最大回数")][SerializeField] private int _maxChargeCount = 3;

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
    [Header("ファンネル")]
    [SerializeField] private Transform _fannel;
    [Header("ファンネルマズル")]
    [SerializeField] private Transform _fannelMuzzle;

    [Header("回転させる中央部分")]
    [SerializeField] private Transform _centerPos;

    [Header("弾を出す位置")]
    [SerializeField] private Transform _muzzlePos;

    [Header("反射の弾")]
    [SerializeField] private GameObject _reflectionbullet;
    [Header("貫通弾")]
    [SerializeField] private GameObject _penetrationBullet;

    [Header("移動転換アタックのコライダー")]
    [SerializeField] private GameObject _moveAttackCollider;
    [Header("移動転換アタックが可能な速度")]
    [SerializeField] private float _moveAttackCanSpeed = 3;
    [Header("移動転換アタックの実行時間")]
    [SerializeField] private float _moveAttackDoTime = 0.5f;



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


    private float _countMoveAttackTime = 0;
    private bool _isDoMoveAttack = false;

    private float _saveInput = 0;

    private bool _isOneCharge = false;

    private int _chargeCount = 0;

    public bool IsDoMoveAttack => _isDoMoveAttack;
    public bool IsCharge => _isCharge;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _chargeEffect.ForEach(i => i.Stop());

        if (_maxChargeCount > 3)
        {
            _maxChargeCount = 3;
        }
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

    public void MoveAttack()
    {
        float h = _playerControl.InputM.HorizontalInput;

        if (_isDoMoveAttack)
        {
            _countMoveAttackTime += Time.deltaTime;

            if (_countMoveAttackTime > _moveAttackDoTime)
            {
                _isDoMoveAttack = false;
                _countMoveAttackTime = 0;
                _moveAttackCollider.SetActive(false);
            }
            return;
        }

        float speed = Mathf.Abs(_playerControl.Rb.linearVelocity.x);

        if (h != _saveInput && h != 0 && speed > _moveAttackCanSpeed)
        {
            _isDoMoveAttack = true;
            _moveAttackCollider.SetActive(true);
            _playerControl.Effect.MoveAttackEffect.SetActive(true);
        }

        _saveInput = h;
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
            Debug.Log("変える");
            return;
        }

        if (_playerControl.InputM.IsLeftMouseClickUp)
        {
            _isReleaseAttackButtun = true;
        }   //攻撃ボタンを離したかどうか

        if (_isOneCharge && _isReleaseAttackButtun)
        {
            Attack();
            //UI_チャージ初期設定
            _playerControl.PlayerUI.ResetChargeUI();
            _isOneCharge = false;
            Debug.Log("ボタンを離した");
            return;
        }


        if (_isCharge)
        {
            //最大ため回数以上はためない
            if (_chargeCount > _maxChargeCount) return;

            _countChargeTime += Time.deltaTime;
            if (_countChargeTime > _attackChageTime)
            {
                _isOneCharge = true;
                int f = _chargeCount + 1;
                _playerControl.PlayerUI.SetGage(f, _attackChageTime, _attackChageTime, true);
                _chargeCount++;
                _countChargeTime = 0;

                //チャージ音_コンプリート
                _playerControl.PlayerSound.ChargeComplete(_chargeCount);

                if (_chargeCount == 3)
                {
                    //チャージ音_最大ため
                    _playerControl.PlayerSound.ChargeKeepSound(true);
                }
                else
                {
                    //チャージ音
                    _playerControl.PlayerSound.ChargeSound(true);
                }


                return;
            }
            int g = _chargeCount + 1;
            _playerControl.PlayerUI.SetGage(g, _attackChageTime, _countChargeTime, false);
        }




        if (_playerControl.InputM.IsLeftMouseClickDown)
        {
            //チャージ音
            _playerControl.PlayerSound.ChargeSound(true);

            _isReleaseAttackButtun = false;
            _isCharge = true;
            _isChangeCamera = true;

            _playerControl.CameraSetting.ChangeCameraPriority(CameraType.Attack);

            if (_chargeEffect.Count > 0)
            {
                _chargeEffect.ForEach(i => i.Play());
            }
        }   //攻撃ボタンを押したかどうか


    }

    /// <summary>ファンネル</summary>
    public void AttackLineRender()
    {

        // **マウスのスクリーン座標を取得**
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Vector3.Distance(Camera.main.transform.position, _centerPos.transform.position);

        // **スクリーン座標をワールド座標に変換**
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // **プレイヤーからマウス方向のベクトルを計算**
        Vector3 direction = (mouseWorldPos - _centerPos.transform.position).normalized;

        // **Y軸の回転のみ適用するため、Y成分を固定**
        direction.y = 0;

        // **オブジェクトをマウスの方向に回転**
        if (direction.sqrMagnitude > 0.01f) // ゼロベクトルを回避
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _centerPos.transform.rotation = targetRotation;
        }

        _lr.SetPosition(0, _fannelMuzzle.position);

        Vector3 pos = _fannelMuzzle.position + (_fannelMuzzle.position - _centerPos.transform.position) * 20;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 worldPos = default;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
        {
            worldPos = hit.point;
            worldPos = new Vector3(worldPos.x, _setYPos, worldPos.z);
            _lr.SetPosition(1, pos);
        }

    }

    public void Attack()
    {
        //チャージ音_最大ため
        _playerControl.PlayerSound.ChargeKeepSound(false);
        //チャージ音
        _playerControl.PlayerSound.ChargeSound(false);

        _isCharge = false;
        _isChangeCamera = false;
        _isCoolTime = false;
        _isReleaseAttackButtun = false;
        _countCoolTime = 0;
        _countChargeTime = 0;

        _chargeEffect.ForEach(i => i.Stop());

        _playerControl.CameraSetting.ResetChangeCameraCount();

        int count = 1;

        if (_chargeCount == 1)
        {
            _playerControl.CameraSetting.ShakeCamera(CameraShakeType.AttackMin);
        }
        if (_chargeCount == 2)
        {
            _playerControl.CameraSetting.ShakeCamera(CameraShakeType.AttackMidium);
            count = 3;
        }
        else if (_chargeCount == 3)
        {
            _playerControl.CameraSetting.ShakeCamera(CameraShakeType.AttackBig);
            count = 5;
        }

        int c = _chargeCount;

        if (c < 0)
        {
            c = 0;
        }
        else if (c > 3)
        {
            c = 3;
        }

        if (_bulletType == BulletType.Reflection)
        {
            //発射音
            _playerControl.PlayerSound.Fire(_chargeCount, false);
            ReflectionbulletSpown(count, c);
        }
        else
        {
            //発射音
            _playerControl.PlayerSound.Fire(_chargeCount, true);
            Penetrati0nBulletSpown(count, c);
        }
        _chargeCount = 0;
    }

    public void Penetrati0nBulletSpown(int i, int c)
    {
        for (int j = 0; j < i; j++)
        {
            GameObject go;
            go = GameObject.Instantiate(_penetrationBullet);
            go.transform.position = _muzzlePos.position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 worldPos = default;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
            {
                worldPos = hit.point;
                worldPos = new Vector3(worldPos.x, _setYPos, worldPos.z);
            }
            Vector3 dir = _fannel.position - _centerPos.position;
            if (j == 1)
            {
                dir = Quaternion.Euler(0, 10, 0) * dir;
            }
            else if (j == 2)
            {
                dir = Quaternion.Euler(0, -10, 0) * dir;
            }
            else if (j == 3)
            {
                dir = Quaternion.Euler(0, 15, 0) * dir;
            }
            else if (j == 4)
            {
                dir = Quaternion.Euler(0, -15, 0) * dir;
            }
            dir.y = 0;

            go?.GetComponent<PlayerBullet>()?.Init(dir, _scoreManager, c - 1);
        }
    }

    public void ReflectionbulletSpown(int i, int c)
    {
        for (int j = 0; j < i; j++)
        {
            GameObject go;
            go = GameObject.Instantiate(_reflectionbullet);
            go.transform.position = _muzzlePos.position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 worldPos = default;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layer))
            {
                worldPos = hit.point;
                worldPos = new Vector3(worldPos.x, _setYPos, worldPos.z);
            }
            Vector3 dir = _fannel.position - _centerPos.position;
            if (j == 1)
            {
                dir = Quaternion.Euler(0, 30, 0) * dir;
            }
            else if (j == 2)
            {
                dir = Quaternion.Euler(0, -30, 0) * dir;
            }
            else if (j == 3)
            {
                dir = Quaternion.Euler(0, 40, 0) * dir;
            }
            else if (j == 4)
            {
                dir = Quaternion.Euler(0, -40, 0) * dir;
            }

            dir.y = 0;


            go?.GetComponent<PlayerBullet>()?.Init(dir, _scoreManager, c - 1);
        }
    }

}
