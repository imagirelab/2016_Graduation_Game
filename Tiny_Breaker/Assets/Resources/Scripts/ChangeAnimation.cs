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
        if (GetComponent<Animator>())
        {
            AnimatorControllerParameter[] parameters = GetComponent<Animator>().parameters;
            foreach (AnimatorControllerParameter param in parameters)
            {
                if(param.name == "IsAttack" && unit.GetComponent<UnitAttack>())
                    GetComponent<Animator>().SetBool("IsAttack", unit.GetComponent<UnitAttack>().IsAttack);
                if (param.name == "IsFind" && unit.GetComponent<UnitSeach>())
                    GetComponent<Animator>().SetBool("IsFind", unit.GetComponent<UnitSeach>().IsFind);
                if (param.name == "IsDead" && unit.GetComponent<Unit>())
                    GetComponent<Animator>().SetBool("IsDead", unit.GetComponent<Unit>().IsDead);
            }
        }
    }
}
