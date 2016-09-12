//参考
//http://qiita.com/ayumegu/items/c07594f408363f73008c

//ドロップされるオブジェクトのクラス

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropObject : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
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
        //色を戻す
        gameObject.GetComponent<Image>().color = Vector4.one;

        //持っていた画像を消えるようにする
        Destroy(pointerEventData.pointerDrag);
    }
}