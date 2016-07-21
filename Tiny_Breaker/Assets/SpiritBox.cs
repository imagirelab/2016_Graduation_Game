using UnityEngine;
using System.Collections;

public class SpiritBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        int count = 0;

        foreach (Transform child in transform)
        {
            //マジックナンバー
            //画像の初期座標と間隔
            child.position = new Vector3(child.position.x,
                                        120.0f - (20.0f * count),
                                        child.position.z);
            if(count == 0)
                child.GetComponent<DragObject>().enabled = true;
            else
                child.GetComponent<DragObject>().enabled = false;

            count++;
        }
    }
}
