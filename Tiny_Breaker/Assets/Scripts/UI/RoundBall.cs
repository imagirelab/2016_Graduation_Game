using UnityEngine;
using System.Collections;
using StaticClass;

public class RoundBall : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] particles = new ParticleSystem[1];
    [SerializeField]
    Color[] playerColors = new Color[GameRule.playerNum];
    [SerializeField]
    Material[] mat = new Material[GameRule.playerNum];

    [SerializeField]
    GameObject[] targetPositions = new GameObject[GameRule.roundCount];
    GameObject target;

    [SerializeField]
    Vector3 startPosition = Vector3.zero;
    public Vector3 StartPosition { get { return startPosition; } set { startPosition = value; } }
    Vector3 moveVec = Vector3.zero;

    [SerializeField]
    float speed = 100.0f;

    bool end = false;
    
    public IEnumerator BallMove()
    {
        end = false;

        #region 準備
        
        int p1wincount = 0;
        int p2wincount = 0;
        foreach (var e in GameRule.getInstance().round)
        {
            if (e == Enum.ResultType.Player1Win)
                p1wincount++;
            if (e == Enum.ResultType.Player2Win)
                p2wincount++;
        }

        //色の決定
        //ターゲットの決定
        if (GameRule.getInstance().round.Count > 0)
            switch (GameRule.getInstance().round[GameRule.getInstance().round.Count - 1])
            {
                case Enum.ResultType.Player1Win:
                    foreach (ParticleSystem e in particles)
                        e.startColor = playerColors[0];
                    GetComponent<MeshRenderer>().material = mat[0];
                    target = targetPositions[p1wincount - 1];
                    break;
                case Enum.ResultType.Player2Win:
                    foreach (ParticleSystem e in particles)
                        e.startColor = playerColors[1];
                    GetComponent<MeshRenderer>().material = mat[1];
                    target = targetPositions[targetPositions.Length - p2wincount];
                    break;
                default:
                    end = true;
                    break;
            }
        else
        {
            foreach (ParticleSystem e in particles)
                e.startColor = playerColors[0];
            GetComponent<MeshRenderer>().material = mat[0];
            target = targetPositions[0];
        }

        //開始位置
        transform.position = startPosition;

        #endregion

        if (!end)
        {
            //集まる
            float[] defaultSize = new float[particles.Length];  //サイズを保存
            for (int i = 0; i < defaultSize.Length; i++)
                defaultSize[i] = particles[i].startSize;
            particles[0].startSize = 0.0f;
            particles[1].startSize = 0.0f;

            yield return new WaitForSeconds(1.5f);

            particles[2].Stop();
            //動くときのパーティクル表示
            particles[0].startSize = defaultSize[0];
            particles[1].startSize = defaultSize[1];
        }

        //動き
        while (!end)
        {
            //方向ベクトル
            moveVec = (target.transform.position - transform.position).normalized;
            GetComponent<Rigidbody>().velocity = moveVec * speed;
            yield return null;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == target)
        {
            if(target.GetComponent<MeshRenderer>())
                target.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
            end = true;
        }
    }
}