using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Transform[] loiteringPointObj;
    public Transform[] LoiteringPointObj { get { return loiteringPointObj; } }

    public int spawn_Num = 1;       //同時生成数
    public int maxSpawns = 10;      //生成数の限界
    public GameObject spawan_Obj;   //生成オブジェクト
    public float spawn_Count = 3;   //生成感覚の時間設定
    public float r = 1;             //生成する場所の半径の距離

    private float timer;
    
	void Start ()
    {
        //設定されてなかった時の代わりの座標
        if(loiteringPointObj.Length == 0)
        {
            loiteringPointObj = new Transform[] 
            {   new GameObject().transform,
                new GameObject().transform,
                new GameObject().transform,
                new GameObject().transform   };

            loiteringPointObj[0].position = transform.position + new Vector3(10.0f, 0.0f, 8.5f);
            loiteringPointObj[1].position = transform.position + new Vector3(-10.0f, 0.0f, 8.5f);
            loiteringPointObj[2].position = transform.position + new Vector3(-10.0f, 0.0f, -8.5f);
            loiteringPointObj[3].position = transform.position + new Vector3(10.0f, 0.0f, -8.5f);

            foreach(Transform e in loiteringPointObj)
                e.transform.parent = gameObject.transform;
        }
    }
	
	void Update ()
    {
        SpownSoldier();
    }

    void SpownSoldier()
    {
        timer += Time.deltaTime;

        //生成時間に達しているか確認
        if (timer > spawn_Count)
        {
            //時間をリセット
            timer = 0;

            int solcount = 0;
            foreach (Transform child in transform)
                if (child.tag == "Enemy/Unit")
                    solcount++;
            
            //最大生成数の上限かを確認
            if (solcount < maxSpawns)
            {
                //生成数までループ
                for (int i = 0; i < spawn_Num; i++)
                {
                    GameObject clone;
                    Vector3 vec = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * r;
                    clone = (GameObject)Instantiate(spawan_Obj, transform.position + vec, spawan_Obj.transform.rotation);
                    clone.transform.parent = gameObject.transform;
                }
            }
        }
    }
}
