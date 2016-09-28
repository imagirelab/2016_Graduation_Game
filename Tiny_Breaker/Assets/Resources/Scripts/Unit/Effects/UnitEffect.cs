using UnityEngine;
using System.Collections;

public class UnitEffect : MonoBehaviour
{
    public enum TYPE
    {
        Demon,
        Soldier
    }
    public TYPE unitType = TYPE.Demon;

    //１個上のデーモンを取得するための置物
    private Demons parentDemon;
    //1個上のソルジャーを取得するための置物
    private Soldier parentSoldier;

    //何回も使うので呼び出しを軽くするために
    private ParticleSystem _particle;

    //再生速度
    public float myplayBackSpeed = 3.0f;

	// Use this for initialization
	void Start ()
    {
        if (unitType == TYPE.Demon)
        {
            //親の方へ向かって最初のDemonsが入ってるオブジェクトを取得
            parentDemon = this.gameObject.GetComponentInParent<Demons>();
        }
        else if(unitType == TYPE.Soldier)
        {
            //親の方へ向かって最初のDemonsが入ってるオブジェクトを取得
            parentSoldier = this.gameObject.GetComponentInParent<Soldier>();
        }

        _particle = this.gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (unitType == TYPE.Demon)
        {
            //ユニットが攻撃判定かつエフェクト再生されてなかった場合に呼び出し
            if (parentDemon.state == Unit.State.Attack && !_particle.isPlaying)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }
            else if (parentDemon.state != Unit.State.Attack)
            {
                _particle.Stop();
                _particle.time = 0;
            }
        }
        else if (unitType == TYPE.Soldier)
        {
            //ユニットが攻撃判定かつエフェクト再生されてなかった場合に呼び出し
            if (parentSoldier.state == Unit.State.Attack && !_particle.isPlaying)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }
            else if (parentSoldier.state != Unit.State.Attack)
            {
                _particle.Stop();
                _particle.time = 0;
            }
        }
    }
}
