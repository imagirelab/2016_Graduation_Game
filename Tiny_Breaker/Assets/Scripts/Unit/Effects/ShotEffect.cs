using UnityEngine;

public class ShotEffect : MonoBehaviour
{
    Unit parentUnit;

    ParticleSystem _particle;

    //再生速度
    public float playBackSpeed = 1.0f;

    [SerializeField]
    public GameObject shotObj;

    [SerializeField]
    Vector3 offset = new Vector3(0, 1.0f, 0);


    [SerializeField, TooltipAttribute("構え時間")]
    float setDelayTime = 0.0f;
    float setCount = 0.0f;

    [SerializeField, Range(0, 1.0f)]
    float shotTimeRate = 0.5f;
    float atkTime = 1.0f;
    float atkCount = 0.0f;

    bool IsShot = false;

    void Start()
    {
        //親の方へ向かって最初のDemonsが入ってるオブジェクトを取得
        parentUnit = this.gameObject.GetComponentInParent<Unit>();

        _particle = this.gameObject.GetComponent<ParticleSystem>();

        setCount = setDelayTime;

        atkTime = parentUnit.status.CurrentAtackTime;
        atkCount = atkTime;

        IsShot = false;
    }

    void Update()
    {
        if (parentUnit.state == Unit.State.Attack)
        {
            if (setCount > 0)
                setCount -= Time.deltaTime;
            else
            {
                atkCount += Time.deltaTime;

                //チャージエフェクトのリセット
                if (atkCount >= atkTime)
                {
                    IsShot = false;

                    atkCount = 0.0f;
                    if (_particle.isPlaying)
                        _particle.Stop();
                    _particle.time = 0;
                    _particle.playbackSpeed = playBackSpeed;
                    _particle.Play();
                }

                //ショットエフェクトの生成
                if (atkCount >= atkTime * shotTimeRate && !IsShot)
                {
                    IsShot = true;

                    GameObject obj = (GameObject)Instantiate(shotObj, parentUnit.transform.position + offset, parentUnit.transform.rotation);
                    obj.transform.parent = parentUnit.transform;
                    obj.layer = parentUnit.transform.gameObject.layer;
                    if (obj.GetComponent<shootEffect>())
                        obj.GetComponent<shootEffect>().Offset = offset;
                }
            }
        }
        else
        {
            setCount = setDelayTime;
        }
    }
}
