//召喚時の動きに関するクラス

using UnityEngine;
using System.Collections;
using SpicyPixel.Threading;
using SpicyPixel.Threading.Tasks;
using System.Threading;

public class SpawnMove : ConcurrentBehaviour
{
    Thread thread;

    bool end = false;   //最後まで終わったかどうかのフラグ
    public bool End { get { return end; } }

    [SerializeField]
    float moveTime = 0.0f;     //召喚時の動く時間
    [SerializeField]
    float stopTime = 0.0f;     //召喚時の止まる時間

    Vector3 targetVec = Vector3.zero;       //召喚時の目標地点までの距離ベクトル
    public Vector3 SetTargetVec { set { targetVec = value; } }

    void OnEnable()
    {
        thread = new Thread(ThreadTemp);
        thread.Start();
    }

    void ThreadTemp()
    {
        taskFactory.StartNew(CoroutineTemp());
    }

    IEnumerator CoroutineTemp()
    {
        //召喚時の動きがあるもの
        //if (unit.setSpawnTargetFlag)
        //{
            ////ステータスUIを消す
            //if (statusUI == null)
            //    statusUI = new GameObject();
            //statusUI.SetActive(false);

            bool moveEnd = false;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + targetVec;
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
                Vector3 rateVec = targetPosition * rate;
                transform.position = startPosition + rateVec;   //座標の代入
                yield return null;
            }
            yield return new WaitForSeconds(stopTime);

        end = true;

        //    //ステータスUIを表示
        //    statusUI.SetActive(true);
        //}
        //GetComponent<SphereCollider>().enabled = true;
        //gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        //yield return null;
    }

}