using UnityEngine;

public class StatusUI : MonoBehaviour
{
    private GameObject parentDemon;
    private GameObject _MainCamera;

	// Use this for initialization
	void Start ()
    {
        parentDemon = transform.root.gameObject;
        _MainCamera = GameObject.Find("Main Camera");

    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(_MainCamera.transform);

        transform.forward = _MainCamera.transform.forward;

        this.GetComponent<TextMesh>().text = "HP:" + parentDemon.GetComponent<Demons>().status.HP + "\nATK:"
            + parentDemon.GetComponent<Demons>().status.ATK + "\nSPEED:" + parentDemon.GetComponent<Demons>().status.SPEED;
	}
}
