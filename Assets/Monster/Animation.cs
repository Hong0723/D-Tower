using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            animator.SetBool("Attack", true);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Walk", false);
        }

    }
}
