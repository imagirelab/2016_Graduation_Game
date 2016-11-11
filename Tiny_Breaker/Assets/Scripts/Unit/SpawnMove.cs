//召喚時の動きに関するクラス

using UnityEngine;
using System.Collections;

public class SpawnMove : MonoBehaviour
{
    bool end;
    public bool End { get { return end; } }

    [SerializeField]
    float moveTime = 0.0f;     //召喚時の動く時間
    [SerializeField]
    float stopTime = 0.0f;     //召喚時の止まる時間

    Vector3 targetVec = Vector3.zero;       //召喚時の目標地点までの距離ベクトル
    public void SetTargetVec(Vector3 vec) { targetVec = vec; }

    Coroutine cor;

    public IEnumerator Spawn()
    {
        end = false;
        //召喚時の動きがあるもの
        //if (unit.setSpawnTargetFlag)
        //{
        ////ステータスUIを消す
        //if (statusUI == null)
        //    statusUI = new GameObject();
        //statusUI.SetActive(false);

        bool moveEnd = false;
        Vector3 startPosition = transform.position;
        float time = 0.0f;
        
        while (moveEnd == false)
        {
            time += Time.deltaTime;
            if (time >= moveTime)
            {
                moveEnd = true;
                time = moveTime;
            }
            float rate = time / moveTime;
            Vector3 rateVec = targetVec * rate;
            transform.position = startPosition + rateVec;   //座標の代入
            yield return null;
        }
        yield return new WaitForSeconds(stopTime);

        //    //ステータスUIを表示
        //    statusUI.SetActive(true);
        //}
        //GetComponent<SphereCollider>().enabled = true;
        //gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        end = true;
    }

    void OnEnable()
    {
        cor = StartCoroutine(Spawn());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}