//悪魔達に与える指示を変更するところ

using UnityEngine;

public class DemonsOrder
{
    //命令
    public enum Order
    {
        Castle,     //城へ行動
        Enemy,      //敵へ行動
        Building,   //建造物へ行動
        Spirit,     //魂を回収させる
        Wait        //待機（一応）
    }
    private Order currentOrder = Order.Wait; //現在の命令
    public Order CurrentOrder { get { return currentOrder; } }  //現在の命令(取得用)

    //ドロップが成立した時の処理
    public void ChangeOrder(Order order)
    {
        //渡された数字からオーダーを変更
        currentOrder = order;
        Debug.Log(currentOrder);
    }
}
