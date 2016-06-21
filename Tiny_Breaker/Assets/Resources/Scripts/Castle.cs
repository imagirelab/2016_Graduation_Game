using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : MonoBehaviour
{
    //お城のの仮ステータス
    public int HP = 1000;
    public int HPpro { get { return HP; } set { HP = value; } }
    public int ATK = 100;
    public float AttackTime = 1.0f;

    private GameObject HPUI;
    private float time;                 //時間
    
    [SerializeField, TooltipAttribute("大砲の発射位置(調整数値)")]
    public Vector3 cannonPosition = Vector3.zero;
    public float shotRange = 1.0f;

    void Start()
    {
        HPUI = GameObject.Find("CastleUI");
    }

    void Update()
    {
        HPUI.GetComponent<TextMesh>().text = "HP: " + HP.ToString();

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
