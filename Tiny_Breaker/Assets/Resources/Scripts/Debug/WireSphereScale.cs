using UnityEngine;

public class WireSphereScale : MonoBehaviour {
    
	void Start () {
    }
	
	void Update () {
        GameObject parent = this.transform.parent.gameObject;
        float colliderDiameter = parent.GetComponent<SphereCollider>().radius * 2.0f;
        this.transform.localScale = new Vector3(colliderDiameter, colliderDiameter, colliderDiameter);
    }
}
