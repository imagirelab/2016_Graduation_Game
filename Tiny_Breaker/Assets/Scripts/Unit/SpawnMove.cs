//召喚時の動きに関するクラス

using UnityEngine;
using System.Collections;

public class SpawnMove : MonoBehaviour
{
    bool end;
    public bool End { get { return end; } }
    
    //スポーンを開始するアニメーター
    public Animator animator;
    //着地エフェクト
    public GameObject effect;

    [SerializeField]
    float spawnTime = 0.0f;     //召喚時の動く時間
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

        bool moveEnd = false;
        Vector3 startPosition = transform.position;
        float time = 0.0f;

        yield return new WaitForSeconds(spawnTime);
        //スポーンアニメーションに切り替え
        if (animator != null)
            animator.SetTrigger("StartBorn");

        while (moveEnd == false)
        {
            time += Time.deltaTime;
            if (time >= moveTime)
            {
                if (effect != null)
                    Instantiate(effect,
                        gameObject.transform.position + effect.transform.position,
                        effect.transform.rotation);

                moveEnd = true;
                time = moveTime;
            }
            float rate = time / moveTime;
            Vector3 rateVec = targetVec * rate;
            transform.position = startPosition + rateVec;   //座標の代入
            yield return null;
        }
        yield return new WaitForSeconds(stopTime);
        
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