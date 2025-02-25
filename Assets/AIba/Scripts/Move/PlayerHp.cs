using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PlayerHp
{

    [Header("体力")]
    [SerializeField] private int _hp;


    [SerializeField] private Text _hpText;

    public int Hp => _hp;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Damage(int damage)
    {
        _hp -= damage;
        _hpText.text = _hp.ToString();

        if (_hp < 0)
        {
            SingletonSceneManager.I.LoadScene("Result");
        }


    }

}
