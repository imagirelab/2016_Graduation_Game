//お城のステータスなどを管理するクラス

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : MonoBehaviour
{
    //お城のステータス
    [SerializeField, TooltipAttribute("体力")]
    int HP = 1000;
    [SerializeField, TooltipAttribute("攻撃力")]
    int ATK = 100;
    [SerializeField, TooltipAttribute("攻撃間隔")]
    float AttackTime = 1.0f;

    [SerializeField, TooltipAttribute("大砲の発射位置(調整数値)")]
    Vector3 cannonPosition = Vector3.zero;

    //外から見れる変数
    public int HPpro { get { return HP; } set { HP = value; } }

    //このクラス内で使う変数
    private GameObject HP_UI;           //HPのUI
    private float time;                 //時間
    
    public float shotRange = 1.0f;

    void Start()
    {
        HP_UI = transform.FindChild("HP").gameObject;
    }

    void Update()
    {
        HP_UI.GetComponent<TextMesh>().text = "HP: " + HP.ToString();

        if (HP <= 0)
        {
            Destroy(gameObject);
        }

        // 弾の発射処理
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        if (units.Length > 0)
        {
            Vector3 nearUnitPosition = units[0].transform.position;
            foreach (var e in units)
                if (Vector3.Distance(transform.position, e.transform.position) < nearUnitPosition.magnitude)
                    nearUnitPosition = e.transform.position;

            if (nearUnitPosition.magnitude < shotRange)
            {
                //アタックタイムを満たしたら
                if (time > AttackTime)
                {
                    time = 0;

                    GameObject cannonPrefab = (GameObject)Resources.Load("Prefabs/Cannon");
                    Instantiate(cannonPrefab,
                                transform.position + cannonPosition,
                                Quaternion.identity);
                }

                time += Time.deltaTime;
            }
        }
    }
}
