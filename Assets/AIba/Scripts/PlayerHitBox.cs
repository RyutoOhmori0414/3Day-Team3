using UnityEngine;

public class PlayerHitBox : MonoBehaviour,IDamageble
{
    [SerializeField] private PlayerControl _playerControl;



    public void AddDamage(int damagePoint)
    {
        _playerControl.Hp.Damage(damagePoint);
    }
}
