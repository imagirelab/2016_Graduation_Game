using UnityEngine;
using System.Collections;

public class Mousecontrol : MonoBehaviour {

	// スクリーン座標をワールド座標に変換した位置座標
	private Vector3 screenToWorldPointPosition;

    //レイキャスト用
    private Ray ray;
    private RaycastHit hit;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // マウス位置座標をスクリーン座標からワールド座標に変換する
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //レイキャストをしてあたり判定の座標を調べる(hitのオブジェクトは取得できてるのでタグを与えられたら目標物へ向かうなどの変更も可)
        if (Physics.Raycast(ray, out hit))
        {
            screenToWorldPointPosition = hit.point + hit.normal * 0.5f;
        }


        //左クリック: 0 右クリック: 1 中ボタン:2
        if (Input.GetMouseButtonDown(0))
        {
            // ワールド座標に変換されたマウス座標を代入
            gameObject.transform.position = new Vector3(screenToWorldPointPosition.x, screenToWorldPointPosition.y + 2.5f, screenToWorldPointPosition.z);

            //移動指示フラグを成立させる
            StaticVariables.goFlag = true;
            //目的地の座標を格納
            StaticVariables.goPosition = screenToWorldPointPosition;
        }
    }
}
