using UnityEngine;

public class ChangeAnimation : MonoBehaviour {

    public Unit unit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(this.GetComponent<Animator>())
            this.GetComponent<Animator>().SetBool("IsAttack", unit.IsAttack);
    }
}
