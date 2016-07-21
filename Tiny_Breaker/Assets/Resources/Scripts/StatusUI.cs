using UnityEngine;

public class StatusUI : MonoBehaviour
{
    private GameObject parent;
    private GameObject _MainCamera;

	// Use this for initialization
	void Start ()
    {
        parent = transform.parent.gameObject;
        _MainCamera = GameObject.Find("Main Camera");

    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(_MainCamera.transform);

        transform.forward = _MainCamera.transform.forward;

        this.GetComponent<TextMesh>().text = "HP:" + parent.GetComponent<Unit>().status.CurrentHP + 
                                            "\nATK:" + parent.GetComponent<Unit>().status.CurrentATK + 
                                            "\nSPEED:" + parent.GetComponent<Unit>().status.CurrentSPEED;
	}
}
