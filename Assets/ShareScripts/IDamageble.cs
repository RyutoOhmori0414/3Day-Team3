using UnityEngine;

public interface IDamageble
{

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damagePoint">与えるダメージの量</param>
    public bool AddDamage(int damagePoint);
}
