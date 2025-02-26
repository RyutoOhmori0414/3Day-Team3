using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseOver()
    {
        animator.SetTrigger("On");
    }
}
