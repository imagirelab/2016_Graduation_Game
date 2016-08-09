using UnityEngine;

public class ChangeAnimation : MonoBehaviour {

    //[SerializeField]
    //Unit unit;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Animator>() && GetComponent<Unit>())
        {
            AnimatorControllerParameter[] parameters = GetComponent<Animator>().parameters;
            foreach (AnimatorControllerParameter param in parameters)
            {
                if(param.name == "IsAttack")
                    GetComponent<Animator>().SetBool("IsAttack", GetComponent<Unit>().IsAttack);
                if (param.name == "IsFind")
                    GetComponent<Animator>().SetBool("IsFind", GetComponent<Unit>().IsFind);
            }
        }
    }
}
