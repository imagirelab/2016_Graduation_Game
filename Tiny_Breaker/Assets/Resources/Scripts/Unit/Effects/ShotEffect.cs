using UnityEngine;
using System.Collections;

public class ShotEffect : MonoBehaviour
{
    public enum TYPE
    {
        Demon,
        Soldier,
        House,
        Base
    }
    public TYPE unitType = TYPE.Demon;

    //１個上のデーモンを取得するための置物
    private Demons parentDemon;
    //1個上のソルジャーを取得するための置物
    private Soldier parentSoldier;

    //何回も使うので呼び出しを軽くするために
    private ParticleSystem _particle;

    //再生速度
    public float myplayBackSpeed = 1.0f;

    public GameObject shotObj;

    public Vector3 offset = new Vector3(0,1.0f,0);

    private float playtime;

    // Use this for initialization
    void Start ()
    {
        if (unitType == TYPE.Demon)
        {
            //親の方へ向かって最初のDemonsが入ってるオブジェクトを取得
            parentDemon = this.gameObject.GetComponentInParent<Demons>();
        }
        else if (unitType == TYPE.Soldier)
        {
            //親の方へ向かって最初のSoldierが入ってるオブジェクトを取得
            parentSoldier = this.gameObject.GetComponentInParent<Soldier>();
        }

        _particle = this.gameObject.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //それぞれコンポーネントの取得先が違うので別々に
        switch (unitType)
        {
            case TYPE.Demon:
            if (!_particle.isPlaying && parentDemon.state == Unit.State.Attack)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }

            if (_particle.isPlaying)
                playtime += Time.deltaTime;

            if (playtime > _particle.startLifetime + _particle.duration && _particle.isPlaying)
            {
                playtime = 0;
                GameObject obj = (GameObject)Instantiate(shotObj, parentDemon.transform.position + offset, parentDemon.transform.rotation);
                obj.transform.parent = parentDemon.transform;
                parentDemon.IsCharge = true;
            }

            break;

            case TYPE.Soldier:
            if (!_particle.isPlaying)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }
            break;

            default:
            break;
        }
    }
}
