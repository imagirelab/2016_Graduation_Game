//目標物を狙う

using UnityEngine;
using System.Collections;

public class UnitSeach : MonoBehaviour
{
    //[SerializeField]    //Unity側から見たいとき用
    [HideInInspector]
    bool isFind = false;
    public bool IsFind { get { return isFind; } }

    //見つける範囲
    public float findRange = 50.0f;

    //見失うまでの時間
    [SerializeField]
    float loseTime = 1.0f;
    float count = 0;
    bool isLose = false;
    public bool IsLose { get { return isLose; } }

    //捉えたターゲット
    //[SerializeField]
    GameObject lookonTarget;
    
    Coroutine cor;
    
    IEnumerator Seach()
    {
        Unit unit = gameObject.GetComponent<Unit>();

        Initialize();

        float widthScale = (transform.localScale.x + transform.localScale.z) / 2.0f;

        while (true)
        {
            if (unit.targetObject != null)
            {
                if (isFind)
                {
                    count += Time.deltaTime;

                    if (count >= loseTime)
                    {
                        Initialize();
                        isLose = true;
                    }
                }

                RaycastHit hit;
                Vector3 vec = unit.targetObject.transform.position - transform.position;
                Ray ray = new Ray(transform.position, vec);
                ray.origin += new Vector3(0.0f, 1.5f, 0.0f);    //視線の高さ分上げている形

                int layerMask = ~(1 << transform.gameObject.layer | 1 << 18);  //自身のレイヤー番号とGround以外にヒットするようにしたビット演算
                if (Physics.SphereCast(ray, 1.5f, out hit, findRange + 1.0f * widthScale, layerMask))   //1.0fはコリジョンの半径
                {
                    if (hit.collider.gameObject == unit.targetObject)
                    {
                        count = 0;
                        isLose = false;
                        isFind = true;
                        lookonTarget = unit.targetObject;
                    }
                }
                ////フラグが切り替わる条件
                //RaycastHit hit;
                //Vector3 vec = unit.targetObject.transform.position - transform.position;
                //Ray ray = new Ray(new Vector3(
                //            transform.position.x,
                //            transform.position.y + 1.5f,    //視線の高さ分上げている形
                //            transform.position.z),
                //            new Vector3(
                //            vec.x,
                //            vec.y + 1.5f,               //視線の高さ分上げている形
                //            vec.z));
                //int layerMask = ~(1 << transform.gameObject.layer | 1 << 18);  //自身のレイヤー番号とGround以外にヒットするようにしたビット演算
                //if (Physics.SphereCast(ray, 1.5f, out hit, findRange + transform.localScale.x, layerMask))
                //{
                //    //捕捉
                //    if (hit.collider.gameObject == unit.targetObject)
                //    {
                //        count = 0;
                //        isFind = true;
                //        lookonTarget = unit.targetObject;
                //    }
                //}

                //狙った敵と本来狙うべき敵に齟齬があった時初期化する
                if (lookonTarget != unit.targetObject)
                    Initialize();

                ////ターゲットを捉えていて、見失っているときの処理
                //if (isFind && lookonTarget == unit.targetObject)
                //{
                //    count += Time.deltaTime;

                //    if (count >= loseTime)
                //        Initialize();
                //}
            }
            yield return null;
        }
    }

    void Initialize()
    {
        count = 0;
        isFind = false;
        lookonTarget = null;
    }

    void OnEnable()
    {
        cor = StartCoroutine(Seach());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}