using UnityEngine;

public class WireBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject parent = this.transform.parent.gameObject;

        Vector3 colliderCenter = parent.GetComponent<BoxCollider>().center;
        this.transform.localPosition = colliderCenter;
        float colliderWidth = parent.GetComponent<BoxCollider>().bounds.size.x * 2.0f;
        float colliderHeight = parent.GetComponent<BoxCollider>().bounds.size.y * 0.5f;
        float colliderDepth = parent.GetComponent<BoxCollider>().bounds.size.z * 2.0f;
        this.transform.localScale = new Vector3(colliderWidth, colliderHeight, colliderDepth);
    }
}
