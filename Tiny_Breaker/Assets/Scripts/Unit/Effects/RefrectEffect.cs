using UnityEngine;
using System.Collections;

public class RefrectEffect : MonoBehaviour
{
    Animator animator;
    AnimatorStateInfo currentState;

    void Start ()
	{
        animator = GetComponent<Animator>();
    }
	
	void Update ()
    {
        currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.shortNameHash == Animator.StringToHash("End"))
        {
            Destroy(transform.gameObject);
        }
    }
}