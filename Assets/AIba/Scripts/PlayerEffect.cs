using UnityEngine;

[System.Serializable]
public class PlayerEffect
{

    [Header("移動の風のエフェクト")]
    [SerializeField] private GameObject _waveEffect;

    [Header("移動の風のエフェクトがでるまでの時間")]
    [SerializeField] private float _waveTime = 2;

    private float _countWaveTime = 0;

    private float _saveInput = 0;

    private bool _isRight = false;

    private bool _isV = false;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void EffectUpdata()
    {
        MoveWave();
    }

    public void MoveWave()
    {
        var h = _playerControl.InputM.HorizontalInput;
        var v = _playerControl.InputM.VerticalInput;

        if (_isRight)
        {
            if (h > 0)
            {
                _isV = false;
            }
        }
        else
        {
            if (h < 0)
            {
                _isV = false;
            }
        }

        if (v != 0 && h == 0)
        {
            _isV = true;
        }

        if ((h > 0 && _saveInput > 0) || h < 0 && _saveInput < 0)
        {
            _countWaveTime += Time.deltaTime;
            if (_countWaveTime > _waveTime && _waveEffect.activeSelf == false)
            {
                if (h > 0)
                {
                    _isRight = true;
                }
                else
                {
                    _isRight = false;
                }

                _waveEffect.SetActive(true);
            }
        }
        else if(_isRight && h<0)
        {
            _waveEffect.SetActive(false);
            _countWaveTime = 0;
        }
        else if(!_isRight && h>0)
        {
            _waveEffect.SetActive(false);
            _countWaveTime = 0;
        }



        _saveInput = h;
    }

}
