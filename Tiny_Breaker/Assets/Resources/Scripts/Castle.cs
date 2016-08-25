//お城を管理するクラス

using UnityEngine;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{
    //兵士が通るルート配列
    [SerializeField]
    GameObject[] rootes;    //一つ目は出現位置。あと巡回ルート
    //List<Transform> rootPointes;
    Dictionary<int, List<Transform>> rootPointes = new Dictionary<int, List<Transform>>();

    //最終目標物
    [SerializeField]
    GameObject target;

    //同時生成数
    [SerializeField, Range(1, 5)]
    int spawnNum = 1;
    //生成間隔
    [SerializeField]
    float spawnCount = 3.0f;
    float timer = 0.0f;

    //兵士たち
    GameObject[] soldiers;

    void Start()
    {
        timer = 0.0f;

        GameObject Ax = (GameObject)Resources.Load("Prefabs/Soldier/SoldierAx");
        GameObject Gun = (GameObject)Resources.Load("Prefabs/Soldier/SoldierGun");
        GameObject Shield = (GameObject)Resources.Load("Prefabs/Soldier/SoldierShield");

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
            //時間をリセット
            timer = spawnCount;

            //兵士の生成
            Spawn();
        }
    }

    void Spawn()
    {
        //ランダムでルートを決定
        int rootNum = Random.Range(0, 100) % 3;

        //生成数までループ
        for (int i = 0; i < spawnNum; i++)
        {
            //ランダムで出撃兵士を決定
            int solNum = Random.Range(0, soldiers.Length);

            GameObject instance = (GameObject)Instantiate(soldiers[solNum],
                                rootes[rootNum].transform.position,
                                soldiers[solNum].transform.rotation);
            instance.transform.parent = gameObject.transform;
            instance.GetComponent<Soldier>().LoiteringPointObj = rootPointes[rootNum].ToArray();
        }
    }
}
