using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MouseControl : MonoBehaviour {

	// スクリーン座標をワールド座標に変換した位置座標
	private Vector3 screenToWorldPointPosition;
    // クリックされたときの座標
    private Vector3 clickPosition;
    public Vector3 ClickPosition { get { return clickPosition; } }
    
    // レイがぶつかったゲームオブジェクト
    private GameObject hitGameObject;
    // クリックされたときのゲームオブジェクト
    private GameObject clickGameObject;
    public GameObject ClickGameObject { get { return clickGameObject; } }

    //レイキャスト用
    private Ray ray;
    private RaycastHit hit;

    //クリックされているかどうか
    private bool isClick;
    public bool IsClick { get { return isClick; } }

    // Use this for initialization
    void Start ()
    {

        screenToWorldPointPosition = Vector3.zero;
        clickPosition = Vector3.zero;

        hitGameObject = new GameObject();
        clickGameObject = new GameObject();

        isClick = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //レイキャストを飛ばすときUIは無視するようにした条件文
        if (!IsPointerOverUIObject())
        {
            //左クリック: 0 右クリック: 1 中ボタン:2
            if (Input.GetMouseButtonDown(0))
            {
                // マウス位置座標をスクリーン座標からワールド座標に変換する
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //レイキャストをしてあたり判定の座標を調べる(hitのオブジェクトは取得できてるのでタグを与えられたら目標物へ向かうなどの変更も可)
                if (Physics.Raycast(ray, out hit))
                {
                    screenToWorldPointPosition = hit.point + hit.normal * 0.5f;

                    // オブジェクトの取得
                    hitGameObject = hit.collider.gameObject;

                    //if (hit.collider.gameObject.name == "Catcher")
                    //{
                    //    StaticVariables.catcherFlag = true;
                    //}

                    // ワールド座標に変換されたマウス座標を代入 (Y座標の2.5fはマジックナンバー？)
                    gameObject.transform.position = new Vector3(screenToWorldPointPosition.x, screenToWorldPointPosition.y + 2.5f, screenToWorldPointPosition.z);

                    // クリックされたときの情報の登録
                    clickPosition = screenToWorldPointPosition;     // 座標
                    clickGameObject = hitGameObject;                // オブジェクト

                    isClick = true;

                    Debug.Log(clickGameObject.name);
                }
            }
            else
                isClick = false;
        }
    }

    private bool IsPointerOverUIObject()
    {
        // http://denshikousaku.net/unity-memo-4　を参考にしました。
        // Referencing this code for GraphicRaycaster https:
        // gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
