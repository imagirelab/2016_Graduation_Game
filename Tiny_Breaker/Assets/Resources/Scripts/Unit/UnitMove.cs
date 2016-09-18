using UnityEngine;
using System.Collections;

public class UnitMove : MonoBehaviour
{
    Coroutine cor;
    
    IEnumerator Move()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        //動くときには回れるように
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;

        while (true)
        {
            if (unit.targetObject != null)
            {
                switch (unit.state)
                {
                    case Unit.State.Search:
                        Vector3 rootPos = unit.GetRootPosition();
                        Vector3 rootVrc = rootPos - transform.position;
                        transform.LookAt(rootPos);
                        gameObject.GetComponent<Rigidbody>().velocity = rootVrc.normalized * unit.loiteringSPEED;
                        break;
                    case Unit.State.Find:
                        Vector3 targetVec = unit.targetObject.transform.position - transform.position;
                        transform.LookAt(unit.targetObject.transform.position);
                        gameObject.GetComponent<Rigidbody>().velocity = targetVec.normalized * unit.status.CurrentSPEED;
                        break;
                    default:
                        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        break;
                }
            }
            else
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            yield return null;
        }
    }
    
    void OnEnable()
    {
        cor = StartCoroutine(Move());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}