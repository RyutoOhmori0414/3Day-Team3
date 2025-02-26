using UnityEngine;

[System.Serializable]
public class PlayerSound
{
    [Header("チャージ")]
    [SerializeField] private AudioSource _charge;
    [Header("チャージ完了_1")]
    [SerializeField] private AudioSource _chargeLevel1;
    [Header("チャージ完了_2")]
    [SerializeField] private AudioSource _chargeLevel2;
    [Header("チャージ完了_3")]
    [SerializeField] private AudioSource _chargeLevel3;
    [Header("チャージ完了_継続")]
    [SerializeField] private AudioSource _chargeLevel3Keeping;

    [Header("発射_Level1")]
    [SerializeField] private AudioSource _fireLevel1;
    [Header("発射_Level2")]
    [SerializeField] private AudioSource _fireLevel2;
    [Header("発射_Level3")]
    [SerializeField] private AudioSource _fireLevel3;

    [Header("発射_貫通")]
    [SerializeField] private AudioSource _firePre;
    [Header("発射_反射")]
    [SerializeField] private AudioSource _fireReflect;

    [Header("ダッシュ")]
    [SerializeField] private AudioSource _dash;
    [Header("ダッシュ2")]
    [SerializeField] private AudioSource _dash2;

    [Header("ダッシュストップ")]
    [SerializeField] private AudioSource _dashStop;

    [Header("ダッシュストップ")]
    [SerializeField] private AudioSource _carb;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    /// <summary>チャージ音</summary>
    /// <param name="isPlay"></param>
    public void CurbSound(bool isPlay)
    {
        if (isPlay)
        {
            _carb.Play();
        }
        else
        {
            _carb.Stop();
        }
    }

    /// <summary>チャージ音</summary>
    /// <param name="isPlay"></param>
    public void ChargeSound(bool isPlay)
    {
        if (isPlay)
        {
            _charge.Play();
        }
        else
        {
            _charge.Stop();
        }
    }

    public void ChargeComplete(int level)
    {
        if (level == 1)
        {
            _chargeLevel1.Play();
        }
        else if (level == 2)
        {
            _chargeLevel2.Play();
        }
        else if (level == 3)
        {
            _chargeLevel3.Play();
        }
    }

    /// <summary>チャージキープの音</summary>
    /// <param name="isPlay"></param>
    public void ChargeKeepSound(bool isPlay)
    {
        if (isPlay)
        {
            _chargeLevel3Keeping.Play();
        }
        else
        {
            _chargeLevel3Keeping.Stop();
        }
    }

    /// <summary>発射音</summary>
    /// <param name="level"></param>
    /// <param name="isPre"></param>
    public void Fire(int level, bool isPre)
    {
        _fireLevel1.Play();
        if (level == 2)
        {
            _fireLevel2.Play();
        }
        else if (level == 3)
        {
            _fireLevel3.Play();
        }

        if (isPre)
        {
            _firePre.Play();
        }
        else
        {
            _fireReflect.Play();
        }
    }


    /// <summary>ダッシュ音</summary>
    /// <param name="isPlay"></param>
    public void Dash(bool isPlay)
    {
        if (isPlay)
        {
            _dash.Play();
            _dash2.Play();
        }
        else
        {
            _dash.Stop();
            _dash2.Stop();
        }
    }

    /// <summary>ダッシュストップ音</summary>
    /// <param name="isPlay"></param>
    public void DashStop(bool isPlay)
    {
        if (isPlay)
        {
            _dashStop.Play();
        }
        else
        {
            _dashStop.Stop();
        }
    }

}
