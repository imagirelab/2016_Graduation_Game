using UnityEngine;
using System.Collections;

public class ChangeAnimation : MonoBehaviour
{
    Coroutine cor;
    
    IEnumerator Change()
    {
        //親
        GameObject parent;

        if (gameObject.transform.parent.gameObject.transform.parent != null)
            parent = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        else
            parent = gameObject;

        Unit unit = parent.GetComponent<Unit>();
        Animator animator = GetComponent<Animator>();
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorControllerParameter[] parameters = animator.parameters;

        Enum.State state;

        state = Enum.State.Search;



        while (true)
        {
            if (animator)
            {
                foreach (AnimatorControllerParameter param in parameters)
                {
                    if (param.name == "IsAttack")
                        animator.SetBool("IsAttack", false);
                    if (param.name == "IsFind")
                        animator.SetBool("IsFind", false);
                    if (param.name == "IsDead")
                        animator.SetBool("IsDead", false);

                    switch (unit.state)
                    {
                        case Enum.State.Attack:
                            if (param.name == "IsAttack")
                                animator.SetBool("IsAttack", true);
                            break;
                        case Enum.State.Find:
                            if (param.name == "IsFind")
                                animator.SetBool("IsFind", true);
                            break;
                        case Enum.State.Dead:
                            if (param.name == "IsDead")
                                animator.SetBool("IsDead", true);
                            break;
                        default:
                            break;
                    }

                    //アニメーションの再生速度
                    if (unit)
                    {
                        switch (unit.state)
                        {
                            case Enum.State.Attack:
                                if (state != Enum.State.Attack &&
                                   currentState.shortNameHash == Animator.StringToHash("attack"))
                                {
                                    state = unit.state;

                                    //Animatorで再生中のAnimationClipのTotal再生時間
                                    float duration = currentState.length;
                                    float animetionTimeRate = duration / unit.status.CurrentAtackTime;
                                    animator.speed = animetionTimeRate;
                                }
                                break;
                            default:
                                state = unit.state;
                                animator.speed = 1.0f;
                                break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    void OnEnable()
    {
        cor = StartCoroutine(Change());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}
