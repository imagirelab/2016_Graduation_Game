//参考
//http://qiita.com/ayumegu/items/c07594f408363f73008c

//ドラッグされるオブジェクトのクラス

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //カンバスのトランスフォーム
    private Transform canvasTran;
    //持ち上げ続ける方のオブジェクト
    private GameObject draggingObject;

    void Start()
    {
        //スマホの方でやるときはスマホの方のCanvasにする
        canvasTran = GameObject.Find("Canvas").transform;
    }

    //ドラッグし始めの処理
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        // ドラッグオブジェクト作成
        CreateDragObject();
        draggingObject.transform.position = pointerEventData.position;
    }

    //ドラッグ中の処理
    public void OnDrag(PointerEventData pointerEventData)
    {
        draggingObject.transform.position = pointerEventData.position;
    }

    //ドラッグが終わった時の処理
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        gameObject.GetComponent<Image>().color = Vector4.one;
        Destroy(draggingObject);
    }

    // ドラッグオブジェクト作成
    private void CreateDragObject()
    {
        draggingObject = new GameObject("Dragging Object");
        draggingObject.transform.SetParent(canvasTran);
        draggingObject.transform.SetAsLastSibling();
        draggingObject.transform.localScale = Vector3.one;

        // レイキャストがブロックされないように
        CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Image draggingImage = draggingObject.AddComponent<Image>();
        Image sourceImage = GetComponent<Image>();

        draggingImage.sprite = sourceImage.sprite;
        draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
        draggingImage.color = sourceImage.color;
        draggingImage.material = sourceImage.material;

        //透明にする(見えなくする)
        gameObject.GetComponent<Image>().color = Vector4.zero;
    }

    //Destoryされたときの処理
    void OnDisable()
    {
        //持ち上げていたオブジェクトを消す
        Destroy(draggingObject);
    }
}