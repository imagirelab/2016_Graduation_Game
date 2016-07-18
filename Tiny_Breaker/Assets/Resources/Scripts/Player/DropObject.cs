//参考
//http://qiita.com/ayumegu/items/c07594f408363f73008c

//ドロップされるオブジェクトのクラス

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropObject : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, TooltipAttribute("悪魔")]
    GameObject demon;

    //成長値の保存変数
    DemonsGrowPointData growPoint;

    void Start()
    {
        //仮に何も設定されていなかったら空のゲームオブジェクトを入れる
        if (demon == null)
            demon = new GameObject();
        else
        {
            //悪魔の成長値を記憶する
            growPoint = demon.GetComponent<Demons>().GrowPoint;
            growPoint.SetGrowPoint();
        }
    }

    //ポインターが入ってきた時の処理
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerDrag == null) return;

        //半透明にしてみる
        gameObject.GetComponent<Image>().color = Vector4.one * 0.5f;
    }

    //ポインターが外に出た時の処理
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerDrag == null) return;

        //色を戻す
        gameObject.GetComponent<Image>().color = Vector4.one;
    }

    //ドロップした時の処理
    public void OnDrop(PointerEventData pointerEventData)
    {
        DemonsGrowPointData spiritGrowPoint = pointerEventData.pointerDrag.GetComponent<IconSpirit>().GrowPoint;

        //成長値の足し方
        growPoint.CurrentHP_GrowPoint += growPoint.GetHP_GrowPoint + spiritGrowPoint.GetHP_GrowPoint;
        growPoint.CurrentATK_GrowPoint += growPoint.GetATK_GrowPoint + spiritGrowPoint.GetATK_GrowPoint;
        growPoint.CurrentSPEED_GrowPoint += growPoint.GetSPEED_GrowPoint + spiritGrowPoint.GetSPEED_GrowPoint;
        growPoint.CurrentAtackTime_GrowPoint += growPoint.GetAtackTime_GrowPoint + spiritGrowPoint.GetAtackTime_GrowPoint;

        demon.GetComponent<Demons>().GrowPoint = growPoint;

        //色を戻す
        gameObject.GetComponent<Image>().color = Vector4.one;

        //持っていた画像を消えるようにする
        Destroy(pointerEventData.pointerDrag);
    }
}