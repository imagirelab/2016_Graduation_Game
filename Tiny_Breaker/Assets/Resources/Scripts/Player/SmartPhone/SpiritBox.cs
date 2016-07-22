using UnityEngine;
using System.Collections;

public class SpiritBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        int count = 0;

        foreach (RectTransform child in transform)
        {
            count++;

            //マジックナンバー
            //画像の初期座標と間隔
            child.position = new Vector3(child.position.x,
                                        2.0f * transform.position.y - child.sizeDelta.y - transform.GetComponent<RectTransform>().sizeDelta.y / 3.0f * count,
                                        child.position.z);
            if(count == 1)
                child.GetComponent<DragObject>().enabled = true;
            else
                child.GetComponent<DragObject>().enabled = false;
            
            //Debug.Log(child.position.y);
        }
    }
}
