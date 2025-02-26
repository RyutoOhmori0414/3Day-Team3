using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerUI 
{

    [Header("チャージのUIを表示する場所")]
    [SerializeField] private Transform _chargeUIPos;

    [Header("チャージのUI_の親")]
    [SerializeField] private RectTransform _chargeUIParent;

    [Header("ゲージ1")]
    [SerializeField] private Image _gageImage1;
    [Header("ゲージ2")]
    [SerializeField] private Image _gageImage2;
    [Header("ゲージ3")]
    [SerializeField] private Image _gageImage3;


    [Header("FillAmount_ゲージ1")]
    [SerializeField] private Image _fillAmountImage1;
    [Header("FillAmount_ゲージ2")]
    [SerializeField] private Image _fillAmountImage2;
    [Header("FillAmount_ゲージ3")]
    [SerializeField] private Image _fillAmountImage3;

    [Header("ゲージ完了_1")]
    [SerializeField] private GameObject _gageChargedImage1;
    [Header("ゲージ完了_2")]
    [SerializeField] private GameObject _gageChargedImage2;
    [Header("ゲージ完了_3")]
    [SerializeField] private GameObject _gageChargedImage3;


    private PlayerControl _playerControl;

    private float _countTime = 0;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _fillAmountImage1.fillAmount = 0;
    }

    public void StartGage()
    {
        _fillAmountImage1.fillAmount = 0;
        _fillAmountImage2.fillAmount = 0;
        _fillAmountImage3.fillAmount = 0;
        _gageImage2.gameObject.SetActive(false);
        _gageImage3.gameObject.SetActive(false);
    }

    public void ResetChargeUI()
    {
        _fillAmountImage1.fillAmount = 0;
        _fillAmountImage2.fillAmount = 0;
        _fillAmountImage3.fillAmount = 0;

        _gageChargedImage1.SetActive(false);
        _gageChargedImage2.SetActive(false);
        _gageChargedImage3.SetActive(false);
        _gageImage2.gameObject.SetActive(false);
        _gageImage3.gameObject.SetActive(false);
    }

    public void SetGage(int gageNum, float chargeTime, float set, bool isComplete)
    {
        if (isComplete)
        {
            if (gageNum == 1)
            {
                _fillAmountImage1.fillAmount = 1;
                _gageChargedImage1.SetActive(true);
                _gageImage2.gameObject.SetActive(true);

            }
            else if (gageNum == 2)
            {
                _fillAmountImage2.fillAmount = 1;
                _gageChargedImage2.SetActive(true);
                _gageImage3.gameObject.SetActive(true);
            }
            else if (gageNum == 3)
            {
                _gageChargedImage3.SetActive(true);
                _fillAmountImage3.fillAmount = 1;
            }
        }
        else
        {
            if (gageNum == 1)
            {
                _fillAmountImage1.fillAmount = Mathf.Clamp01(set / chargeTime);
            }
            else if (gageNum == 2)
            {
                _fillAmountImage2.fillAmount = Mathf.Clamp01(set / chargeTime);
            }
            else if (gageNum == 3)
            {
                _fillAmountImage3.fillAmount = Mathf.Clamp01(set / chargeTime);
            }
        }
    }



    public void ChargeUIPosition()
    {
        // **ワールド座標をスクリーン座標に変換**
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_chargeUIPos.position);

        // **スクリーン内に収まっているか確認（Zが負の場合はカメラの後ろなので非表示）**
        if (screenPos.z > 0)
        {
            _chargeUIParent.gameObject.SetActive(true);
            _chargeUIParent.position = screenPos; // UIの位置をスクリーン座標に設定
        }
    }

}
