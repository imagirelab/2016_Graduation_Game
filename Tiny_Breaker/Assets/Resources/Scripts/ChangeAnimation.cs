using UnityEngine;

public class ChangeAnimation : MonoBehaviour {

    //[SerializeField]
    //Unit unit;

    //親
    GameObject unit;
    
	void Start () {
        if (gameObject.transform.parent.gameObject.transform.parent != null)
            unit = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        else
            unit = gameObject;
    }
	
	void Update () {
        if (GetComponent<Animator>() && unit.GetComponent<Unit>())
        {
            AnimatorControllerParameter[] parameters = GetComponent<Animator>().parameters;
            foreach (AnimatorControllerParameter param in parameters)
            {
                if(param.name == "IsAttack")
                    GetComponent<Animator>().SetBool("IsAttack", unit.GetComponent<Unit>().IsAttack);
                if (param.name == "IsFind")
                    GetComponent<Animator>().SetBool("IsFind", unit.GetComponent<Unit>().IsFind);
            }
        }
    }
}
