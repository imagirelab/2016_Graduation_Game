// 作成者　田中
// プレイヤーが出せる指令に関するクラス
// （移動指示や破壊命令など）

using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    //命令
    public enum Order
    {
        Move,       //移動
        Building,   //建造物へ行動
        Enemy,      //敵へ行動
        Wait        //待機（一応）
    }
    private Order currentOrder; //現在の命令
    public Order CurrentOrder { get { return currentOrder; } }  //現在の命令(取得用)

    //このクラス内で使う変数
    private GameObject fieldCommand;    //フィールドでの指示

    // 初期化
    void Start () {

        // 命令の初期設定
        currentOrder = Order.Wait;

        // Resourcesフォルダからプレハブ情報の取得
        GameObject fieldCommandPrefab = (GameObject)Resources.Load("Prefabs/CommandTarget");
        // //とりあえずそのままインスタンス化
        fieldCommand = (GameObject)Instantiate( fieldCommandPrefab,
                                                fieldCommandPrefab.transform.position,
                                                Quaternion.identity);
        
    }
	
	// 更新
	void Update () {

        // 命令が切り替わる条件
        switch (fieldCommand.GetComponent<MouseControl>().ClickGameObject.tag)
        {
            case "Ground":
                currentOrder = Order.Move;
                break;
            case "Building":
                currentOrder = Order.Building;
                break;
            case "Enemy":
                currentOrder = Order.Enemy;
                break;
            default:
                currentOrder = Order.Wait;
                break;
        }

	}
}
