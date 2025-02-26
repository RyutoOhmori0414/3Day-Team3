using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PlayerHp
{

    [Header("体力")]
    [SerializeField] private int _hp;

    [Header("無敵時間")]
    [SerializeField] private float _godTime = 2;

    private float _countGod = 0;
    private bool _isDamage = false;


    [SerializeField] private Text _hpText;

    [SerializeField] private ScoreManager _scoreManager;
    public int Hp => _hp;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void CountGodTime()
    {
        if (_isDamage)
        {
            _countGod += Time.deltaTime;

            if (_countGod > _godTime)
            {
                _countGod = 0;
                _isDamage = false;
            }
        }
    }

    public void Damage(int damage)
    {
        if (_isDamage) return;

        _hp -= damage;
        _hpText.text = _hp.ToString();
        _isDamage = true;
        _scoreManager.DissScore();

        _playerControl.PlayerSound.Damage();
        if (_hp < 0)
        {
            GameManager.I.StopGame();
        }
    }

}
