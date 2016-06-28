// 作成者　田中
// 悪魔達全体の出撃数や指示などを管理するクラス

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//悪魔達の初期情報(structの代わりにclassを使用)
[System.Serializable]
public class DemonData
{
    [SerializeField, Range(0, 9), TooltipAttribute("初期出撃数")]
    public int num;
    [SerializeField, TooltipAttribute("悪魔の種類")]
    public GameObject gameObuject;
}

public class DemonsManager : MonoBehaviour
{
    public DemonData PUPU;  //ププ
    public DemonData POPO;  //ポポ
    public DemonData PIPI;  //ピピ

    private GameObject fieldCommand;

    // Use this for initialization
    void Start () {

        // 悪魔たちを召喚
        InstantiateDemons(PUPU);
        InstantiateDemons(POPO);
        InstantiateDemons(PIPI);

        //目的地のゲームオブジェクト情報を手に入れるためフィールドに指示された地点の情報を取っている
        GameObject fieldCommand = GameObject.FindGameObjectWithTag("Player/Command");
    }

    // ユニットのインスタンス化
    void InstantiateDemons(DemonData demonData)
    {
        for (int i = 0; i < demonData.num; i++)
        {
            demonData.gameObuject.GetComponent<Demons>().HPpro = demonData.gameObuject.GetComponent<Demons>().defaultHPpro;
            demonData.gameObuject.GetComponent<Demons>().ATKpro = demonData.gameObuject.GetComponent<Demons>().defaultATKpro;
            demonData.gameObuject.GetComponent<Demons>().SPEEDpro = demonData.gameObuject.GetComponent<Demons>().defaultSPEEDpro;

            // プレハブのインスタンス化
            // xyはそのままにz軸方向にだけ少しずつずらしている
            Instantiate(demonData.gameObuject,
                        new Vector3(demonData.gameObuject.transform.position.x, demonData.gameObuject.transform.position.y, demonData.gameObuject.transform.position.z - i * 1),
                        Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {

        //プレイヤーの命令と悪魔たちの情報を取るためにFindで探す
        GameObject playerOrder = GameObject.FindGameObjectWithTag("Player");
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        //プレイヤーの現在の命令によって更新を変えている
        switch (playerOrder.GetComponent<PlayerCommand>().CurrentOrder)
        {
            case PlayerCommand.Order.Move:
                MoveOrder(units);
                break;
            case PlayerCommand.Order.Building:
                BuildingOrder(units);
                break;
            case PlayerCommand.Order.Enemy:
                EnemyOrder(units);
                break;
            default:
                WaitOrder(units);
                break;
        }

    }
    
    // 悪魔達全体の移動命令処理
    void MoveOrder(GameObject[] units)
    {
        //目的地のゲームオブジェクト情報を手に入れるためフィールドに指示された地点の情報を取っている
        GameObject fieldCommand = GameObject.FindGameObjectWithTag("Player/Command");

        foreach (GameObject e in units)
            //悪魔クラスを持っていたら処理
            if (e.GetComponent<Demons>() != null)
                e.GetComponent<Demons>().MoveOrder(fieldCommand.GetComponent<Mousecontrol>().ClickPosition);
    }

    // 悪魔達全体の建造物に向かい攻撃する命令の処理
    void BuildingOrder(GameObject[] units)
    {
        //目的地のゲームオブジェクト情報を手に入れるためフィールドに指示された地点の情報を取っている
        GameObject fieldCommand = GameObject.FindGameObjectWithTag("Player/Command");

        foreach (var e in units)
            //悪魔クラスを持っていたら処理
            if(e.GetComponent<Demons>() != null)
                e.GetComponent<Demons>().BuildingOrder(fieldCommand.GetComponent<Mousecontrol>().ClickGameObject);
    }

    // 悪魔達全体の敵に攻撃する命令の処理
    void EnemyOrder(GameObject[] units)
    {
        //目的地のゲームオブジェクト情報を手に入れるためフィールドに指示された地点の情報を取っている
        GameObject fieldCommand = GameObject.FindGameObjectWithTag("Player/Command");

        foreach (var e in units)
            //悪魔クラスを持っていたら処理
            if (e.GetComponent<Demons>() != null)
                e.GetComponent<Demons>().EnemyOrder(fieldCommand.GetComponent<Mousecontrol>().ClickGameObject);
    }

    // 悪魔達全体の待機命令の処理(いらないかもしれないが)
    void WaitOrder(GameObject[] units)
    {
        foreach (var e in units)
            //悪魔クラスを持っていたら処理
            if (e.GetComponent<Demons>() != null)
                e.GetComponent<Demons>().WaitOrder();
    }
}
