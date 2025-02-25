using UnityEngine;
/// <summary>
/// Debug用なので消してもいい
/// </summary>
public class DebugHP : MonoBehaviour
{
    [SerializeField] float _life = 10;
    float _moveSpeed = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_life <= 0)
        {
            Destroy(gameObject);
        }
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(h, 0, v) * _moveSpeed * Time.deltaTime;
    }
    public void AddDamage(float damage)
    {
        _life -= damage;
    }
}
