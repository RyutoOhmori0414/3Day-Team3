using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    [Header("攻撃")]
    [SerializeField] private int _attackPower = 5;
    [SerializeField] private ScoreManager _scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other?.GetComponent<IDamageble>()?.AddDamage(_attackPower);
            _scoreManager.AddScore(0, other.gameObject.transform);
        }
    }
}