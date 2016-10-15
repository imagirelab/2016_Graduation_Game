using UnityEngine;
using System.Collections;

public class ChangeAnimation : MonoBehaviour
{
    Coroutine cor;

    //親
    GameObject unit;

    Unit.State state;
    
    IEnumerator Change()
    {
        if (gameObject.transform.parent.gameObject.transform.parent != null)
            unit = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        else
            unit = gameObject;

        state = Unit.State.Search;

        while (true)
        {
            if (GetComponent<Animator>())
            {
                AnimatorControllerParameter[] parameters = GetComponent<Animator>().parameters;
                foreach (AnimatorControllerParameter param in parameters)
                {
                    if (param.name == "IsAttack" && unit.GetComponent<UnitAttack>())
                        GetComponent<Animator>().SetBool("IsAttack", unit.GetComponent<UnitAttack>().IsAttack);
                    if (param.name == "IsFind" && unit.GetComponent<UnitSeach>())
                        GetComponent<Animator>().SetBool("IsFind", unit.GetComponent<UnitSeach>().IsFind);
                    if (param.name == "IsDead" && unit.GetComponent<Unit>())
                        GetComponent<Animator>().SetBool("IsDead", unit.GetComponent<Unit>().IsDead);

                    //アニメーションの再生速度
                    if (unit.GetComponent<Unit>())
                    {
                        Unit unitComp = unit.GetComponent<Unit>();
                        AnimatorStateInfo currentState = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

                        switch (unitComp.state)
                        {
                            case Unit.State.Attack:
                                if (state != Unit.State.Attack &&
                                   currentState.shortNameHash == Animator.StringToHash("attack"))
                                {
                                    state = unitComp.state;

                                    //Animatorで再生中のAnimationClipのTotal再生時間
                                    float duration = currentState.length;
                                    float animetionTimeRate = duration / unit.GetComponent<Unit>().status.CurrentAtackTime;
                                    GetComponent<Animator>().speed = animetionTimeRate;
                                }
                                break;
                            default:
                                state = unitComp.state;
                                GetComponent<Animator>().speed = 1.0f;
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
