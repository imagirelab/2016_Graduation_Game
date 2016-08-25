using UnityEngine;

public class WireSphere : MonoBehaviour
{

    void Start()
    {
    }

    void Update()
    {
        GameObject parent = this.transform.parent.gameObject;

        Vector3 colliderCenter = parent.GetComponent<SphereCollider>().center;
        this.transform.localPosition = colliderCenter;
        float colliderDiameter = parent.GetComponent<SphereCollider>().radius * 2.0f;
        this.transform.localScale = new Vector3(colliderDiameter, colliderDiameter, colliderDiameter);
    }
}
