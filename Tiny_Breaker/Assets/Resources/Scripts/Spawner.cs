using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public int spawn_Num = 1;       //生成数
    public GameObject spawan_Obj;   //生成オブジェクト
    public float spawn_Count = 3;   //生成感覚の時間設定
    public float r = 1;             //生成する場所の半径の距離

    private float timer;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        //生成時間に達しているか確認
        if(timer > spawn_Count)
        {
            //時間をリセット
            timer = 0;

            //生成数までループ
            for(int i = 0; i < spawn_Num; i++)
            {
                Vector3 vec = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * r;
                Instantiate(spawan_Obj, transform.position + vec, spawan_Obj.transform.rotation);
            }
        }
	}
}
