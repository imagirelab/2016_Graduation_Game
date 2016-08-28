//お城を管理するクラス

using UnityEngine;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{
    //兵士が通るルート配列
    [SerializeField]
    GameObject[] rootes;    //一つ目は出現位置。あと巡回ルート
    Dictionary<int, List<Transform>> rootPointes = new Dictionary<int, List<Transform>>();

    //最終目標物
    [SerializeField]
    public GameObject target;

    //相手のタグ
    [SerializeField]
    string tergetTag = "";

    //同時生成数
    [SerializeField, Range(1, 5)]
    int spawnNum = 1;
    //生成間隔
    [SerializeField]
    float spawnCountRange = 3.0f;
    float timer = 0.0f;

    //兵士たち
    GameObject[] soldiers;

    void Start()
    {
        timer = 0.0f;

        GameObject Ax = (GameObject)Resources.Load("Prefabs/Soldier/SoldierAx");
        Ax.GetComponent<Unit>().status.SetStatus();
        GameObject Gun = (GameObject)Resources.Load("Prefabs/Soldier/SoldierGun");
        Gun.GetComponent<Unit>().status.SetStatus();
        GameObject Shield = (GameObject)Resources.Load("Prefabs/Soldier/SoldierShield");
        Shield.GetComponent<Unit>().status.SetStatus();

        soldiers = new GameObject[] { Ax, Gun, Shield };

        if (rootes == null)
            rootes = new GameObject[] { transform.gameObject };
        if (target == null)
            target = transform.gameObject;

        //巡回ルートの作成
        for (int i = 0; i < rootes.Length; i++)
        {
            rootPointes.Add(i, new List<Transform>());

            foreach (Transform child in rootes[i].transform)
                rootPointes[i].Add(child);
            rootPointes[i].Add(target.transform); //最後に最終目的地
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer < 0.0f)
        {
            //生成にムラを出す
            float spawnCount = Random.Range(1.0f, spawnCountRange);

            //時間をリセット
            timer = spawnCount;

            //兵士の生成
            Spawn();
        }
    }

    void Spawn()
    {
        //ランダムでルートを決定
        int rootNum = Random.Range(0, rootes.Length);

        //生成数までループ
        for (int i = 0; i < spawnNum; i++)
        {
            //ランダムで出撃兵士を決定
            int solNum = Random.Range(0, soldiers.Length);

            GameObject instance = (GameObject)Instantiate(soldiers[solNum],
                                rootes[rootNum].transform.position,
                                soldiers[solNum].transform.rotation);
            instance.tag = transform.gameObject.tag;    //自分のタグを設定
            instance.GetComponent<Unit>().targetTag = tergetTag;   //相手のタグを設定
            instance.transform.parent = gameObject.transform;
            instance.GetComponent<Soldier>().LoiteringPointObj = rootPointes[rootNum].ToArray();

            //適当に何体かに１体強化個体を作る
            int powerUPRand = Random.Range(0, 10);
            if (powerUPRand == 0)
            {
                instance.GetComponent<Soldier>().powerUPCount = 8;
                instance.transform.localScale *= 2.0f;
            }
        }
    }
}
