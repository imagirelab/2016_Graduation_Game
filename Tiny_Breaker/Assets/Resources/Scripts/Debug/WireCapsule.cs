using UnityEngine;

public class WireCapsule : MonoBehaviour {
    
	void Start () {
	
	}
	
	void Update () {
        GameObject parent = this.transform.parent.gameObject;

        //ポジション
        Vector3 colliderCenter = parent.GetComponent<CapsuleCollider>().center;
        transform.localPosition = colliderCenter;
        //ローテーション
        switch(parent.GetComponent<CapsuleCollider>().direction)
        {
            case 0: //X軸
                transform.localRotation.Set(
                    transform.localRotation.x,
                    transform.localRotation.y,
                    transform.localRotation.z + 90.0f,
                    transform.localRotation.w);
                break;
            case 1: //Y軸
                transform.localRotation.Set(
                    transform.localRotation.x,
                    transform.localRotation.y,
                    transform.localRotation.z,
                    transform.localRotation.w);
                break;
            case 2: //Z軸
                transform.localRotation.Set(
                    transform.localRotation.x + 90.0f,
                    transform.localRotation.y,
                    transform.localRotation.z,
                    transform.localRotation.w);
                break;
        }
        //スケール
        float colliderRadius = parent.GetComponent<CapsuleCollider>().radius * 2.0f;
        float colliderHeight = parent.GetComponent<CapsuleCollider>().height * 0.5f;
        transform.localScale = new Vector3(colliderRadius, colliderHeight, colliderRadius);
    }
}
