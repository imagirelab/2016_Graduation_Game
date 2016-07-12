using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public int spawn_Num = 1;       //同時生成数
    public int maxSpawns = 10;      //生成数の限界
    public GameObject spawan_Obj;   //生成オブジェクト
    public float spawn_Count = 3;   //生成感覚の時間設定
    public float r = 1;             //生成する場所の半径の距離

    private float timer;

    private GameObject clone;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
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

            //最大生成数の上限かを確認
            if (this.gameObject.transform.childCount < maxSpawns)
            {
                //生成数までループ
                for (int i = 0; i < spawn_Num; i++)
                {
                    Vector3 vec = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * r;
                    clone = (GameObject)Instantiate(spawan_Obj, transform.position + vec, spawan_Obj.transform.rotation);
                    clone.transform.parent = this.gameObject.transform;
                }
            }
        }
    }
}
