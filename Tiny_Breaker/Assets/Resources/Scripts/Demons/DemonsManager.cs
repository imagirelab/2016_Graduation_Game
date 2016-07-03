// 作成者　田中
// 悪魔達全体の出撃や指示などを管理するクラス

using UnityEngine;

public class DemonsManager : MonoBehaviour
{
    private GameObject[] units;

    void Start () {
        units = null;
    }
    
    // Update is called once per frame
    void Update () {
        
        //プレイヤーの命令と悪魔たちの情報を取るためにFindで探す
        units = GameObject.FindGameObjectsWithTag("Unit");
        if (units != null)
        {
            //プレイヤーの現在の命令によって更新を変えている
            switch (this.GetComponent<PlayerControl>().CurrentOrder)
            {
                case PlayerControl.Order.Move:
                    MoveOrder();
                    break;
                case PlayerControl.Order.Building:
                    BuildingOrder();
                    break;
                case PlayerControl.Order.Enemy:
                    EnemyOrder();
                    break;
                case PlayerControl.Order.Summon:    //召喚中の他の悪魔達の行動
                    MoveOrder();
                    break;
                default:
                    WaitOrder();
                    break;
            }
        }
    }
    
    // 悪魔達全体の移動命令処理
    void MoveOrder()
    {
        foreach (GameObject e in units)
            e.GetComponent<Demons>().MoveOrder(this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickPosition);
    }

    // 悪魔達全体の建造物に向かい攻撃する命令の処理
    void BuildingOrder()
    {
        foreach (var e in units)
                e.GetComponent<Demons>().BuildingOrder(this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickGameObject);
    }

    // 悪魔達全体の敵に攻撃する命令の処理
    void EnemyOrder()
    {
        foreach (var e in units)
                e.GetComponent<Demons>().EnemyOrder(this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickGameObject);
    }

    // 悪魔達全体の待機命令の処理(いらないかもしれないが)
    void WaitOrder()
    {
        foreach (var e in units)
            if (e.GetComponent<Demons>() != null)
                e.GetComponent<Demons>().WaitOrder();
    }
}