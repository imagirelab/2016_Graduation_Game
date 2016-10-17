//目標物を狙う

using UnityEngine;

public class UnitSeach : MonoBehaviour
{
    //[SerializeField]    //Unity側から見たいとき用
    [HideInInspector]
    bool isFind = false;
    public bool IsFind { get { return isFind; } }

    //見つける範囲
    [SerializeField]
    float findRange = 50.0f;

    //見失うまでの時間
    [SerializeField]
    float loseTime = 1.0f;
    float count = 0;

    //捉えたターゲット
    [SerializeField]
    GameObject lookonTarget;

    Unit unit;

    void Start()
    {
        unit = gameObject.GetComponent<Unit>();

        Initialize();
    }

    void Update()
    {
        if (unit.targetObject != null)
            if (Vector3.Distance(this.transform.position, unit.targetObject.transform.position) < 3.0f) //3.0f = 重なっているとする距離
            {
                count = 0;
                isFind = true;
                lookonTarget = unit.targetObject;
            }
            else
            {
                //フラグが切り替わる条件
                RaycastHit hit;
                Vector3 vec = unit.targetObject.transform.position - transform.position;
                Ray ray = new Ray(new Vector3(
                            transform.position.x,
                            transform.position.y + 1.5f,    //視線の高さ分上げている形
                            transform.position.z),
                            new Vector3(
                            vec.x,
                            vec.y + 1.5f,               //視線の高さ分上げている形
                            vec.z));
                int layerMask = ~(1 << transform.gameObject.layer | 1 << 18);  //自身のレイヤー番号とGround以外にヒットするようにしたビット演算
                if (Physics.SphereCast(ray, 1.5f, out hit, findRange + transform.localScale.x, layerMask))
                {
                    if (hit.collider.gameObject == unit.targetObject)
                    {
                        count = 0;
                        isFind = true;
                        lookonTarget = unit.targetObject;
                    }
                }
            }

        //狙った敵と本来狙うべき敵に齟齬があった時初期化する
        if (lookonTarget != unit.targetObject)
            Initialize();

        //ターゲットを捉えていて、見失っているときの処理
        if (!isFind && lookonTarget == unit.targetObject)
        {
            count += Time.deltaTime;

            if (count >= loseTime)
                Initialize();
        }
    }

    void Initialize()
    {
        count = 0;
        isFind = false;
        lookonTarget = null;
    }
}