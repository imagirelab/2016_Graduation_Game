// 作成者　田中
// 悪魔達全体の出撃数や指示などを管理するクラス

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

////悪魔達の初期情報(structの代わりにclassを使用)
//[System.Serializable]
//public class DemonData
//{
//    //[SerializeField, TooltipAttribute("悪魔の種類")]
//    //public GameObject gameObuject;
    
//    [SerializeField, TooltipAttribute("体力")]
//    public int HP = 100;
//    [SerializeField, TooltipAttribute("攻撃力")]
//    public int ATK = 100;
//    [SerializeField, TooltipAttribute("速度")]
//    public int SPEED = 5;
//    [SerializeField, TooltipAttribute("攻撃間隔")]
//    public float AtackTime = 1.0f;

//    private int MaxHP;
//    public int GetMaxHP { get { return MaxHP; } }

//    DemonData() { MaxHP = HP; }
//}

public class DemonsManager : MonoBehaviour
{
    [SerializeField, TooltipAttribute("ププ")]
    private GameObject PUPU;  //ププ
    [SerializeField, TooltipAttribute("ポポ")]
    private GameObject POPO;  //ポポ
    [SerializeField, TooltipAttribute("ピピ")]
    private GameObject PIPI;  //ピピ
    
    void Start () {

        // 悪魔たちを召喚
        //InstantiateDemons(PUPU);
        //InstantiateDemons(POPO);
        //InstantiateDemons(PIPI);
    }

    // ユニットのインスタンス化
    //void InstantiateDemons(GameObject gameObuject)
    //{
    //    // プレハブのインスタンス化
    //    Instantiate(gameObuject,
    //                gameObuject.transform.position,
    //                Quaternion.identity);
    //}
	
	// Update is called once per frame
	void Update () {

        //プレイヤーの命令と悪魔たちの情報を取るためにFindで探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        //プレイヤーの現在の命令によって更新を変えている
        switch (player.GetComponent<PlayerControl>().CurrentOrder)
        {
            case PlayerControl.Order.Move:
                MoveOrder(player, units);
                break;
            case PlayerControl.Order.Building:
                BuildingOrder(player, units);
                break;
            case PlayerControl.Order.Enemy:
                EnemyOrder(player, units);
                break;
            default:
                WaitOrder(units);
                break;
        }

    }
    
    // 悪魔達全体の移動命令処理
    void MoveOrder(GameObject player, GameObject[] units)
    {
        foreach (GameObject e in units)
                e.GetComponent<Demons>().MoveOrder(player.GetComponent<Mousecontrol>().ClickPosition);
    }

    // 悪魔達全体の建造物に向かい攻撃する命令の処理
    void BuildingOrder(GameObject player, GameObject[] units)
    {
        foreach (var e in units)
                e.GetComponent<Demons>().BuildingOrder(player.GetComponent<Mousecontrol>().ClickGameObject);
    }

    // 悪魔達全体の敵に攻撃する命令の処理
    void EnemyOrder(GameObject player, GameObject[] units)
    {
        foreach (var e in units)
                e.GetComponent<Demons>().EnemyOrder(player.GetComponent<Mousecontrol>().ClickGameObject);
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