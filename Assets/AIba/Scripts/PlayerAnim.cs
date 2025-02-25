using System;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    [Header("Playerのアニメーション")]
    [SerializeField] private Animator _animator;
    [Header("Playerのアニメーション_ベース")]
    [SerializeField] private Animator _animatorBase;


    [Header("PlayerImage")]
    [SerializeField] private Transform _playerImage;
    [Header("PlayerImage_Offset変更用")]
    [SerializeField] private Transform _playerImageOffsetChange;
    [Header("OffsetX")]
    [SerializeField] private float _playerImageOffset;
    [Header("OffsetX変更速度")]
    [SerializeField] private float _playerImageOffsetChangeSpeed = 0.1f;

    [Header("Offsetを素早く変更すると判断する差")]
    [SerializeField] private float _offSetDis = 6;
    [Header("OffsetX変更速度_早い")]
    [SerializeField] private float _playerImageOffsetChangeSpeedFast = 0.1f;

    [Header("OffsetX変更速度_戻る")]
    [SerializeField] private float _playerImageOffsetChangeSpeedBack = 0.1f;


    private float _saveInputH = 0;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Move(bool isMove)
    {
        _animator.SetBool("Move", isMove);
        _animatorBase.SetBool("Move", isMove);
    }


    public void AnimUpdata()
    {
        SetScale();
        Offset();
    }

    public void SetScale()
    {
        float i = _playerControl.InputM.HorizontalInput;

        if (i > 0)
        {
            _playerImage.localScale = new Vector3(1, 1, 1);
        }
        else if (i < 0)
        {
            _playerImage.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Offset()
    {
        float h = _playerControl.InputM.HorizontalInput;

        if (h > 0)
        {
            if (_playerImageOffsetChange.transform.localPosition.x > _playerImageOffset)
            {
                float speed = _playerImageOffsetChangeSpeed;
                if(MathF.Abs(_playerImageOffset - _playerImageOffsetChange.transform.localPosition.x) > _offSetDis)
                {
                    speed = _playerImageOffsetChangeSpeedFast;
                }

                float setX = _playerImageOffsetChange.transform.localPosition.x - (Time.deltaTime * speed);


                if (setX < _playerImageOffset)
                {
                    setX = _playerImageOffset;
                }



                _playerImageOffsetChange.localPosition = new Vector3(setX, _playerImageOffsetChange.localPosition.y, _playerImageOffsetChange.localPosition.z);
            }
        }
        else if (h < 0)
        {
            if (_playerImageOffsetChange.transform.localPosition.x < -_playerImageOffset)
            {
                float speed = _playerImageOffsetChangeSpeed;
                if (MathF.Abs(-_playerImageOffset - _playerImageOffsetChange.transform.localPosition.x) > _offSetDis)
                {
                    speed = _playerImageOffsetChangeSpeedFast;
                }


                float setX = _playerImageOffsetChange.transform.localPosition.x + Time.deltaTime * speed;

                if (setX > -_playerImageOffset)
                {
                    setX = -_playerImageOffset;
                }

                _playerImageOffsetChange.localPosition = new Vector3(setX, _playerImageOffsetChange.localPosition.y, _playerImageOffsetChange.localPosition.z);

            }

        }
        else
        {
            if (_playerImageOffsetChange.transform.localPosition.x > 0)
            {
                float setX = _playerImageOffsetChange.transform.localPosition.x - (Time.deltaTime * _playerImageOffsetChangeSpeedBack);

                if (setX < 0)
                {
                    setX = 0;
                }

                _playerImageOffsetChange.localPosition = new Vector3(setX, _playerImageOffsetChange.localPosition.y, _playerImageOffsetChange.localPosition.z);
            }
            else if (_playerImageOffsetChange.transform.localPosition.x < 0)
            {
                float setX = _playerImageOffsetChange.transform.localPosition.x + (Time.deltaTime * _playerImageOffsetChangeSpeedBack);

                if (setX > 0)
                {
                    setX = 0;
                }
                _playerImageOffsetChange.localPosition = new Vector3(setX, _playerImageOffsetChange.localPosition.y, _playerImageOffsetChange.localPosition.z);
            }
        }


        _saveInputH = h;
    }

}
