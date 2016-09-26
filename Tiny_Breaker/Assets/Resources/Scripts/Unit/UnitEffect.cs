using UnityEngine;
using System.Collections;

public class UnitEffect : MonoBehaviour
{
    //１個上のデーモンを取得するための置物
    private Demons parentDemon;

    //何回も使うので呼び出しを軽くするために
    private ParticleSystem _particle;

    //再生速度
    public float myplayBackSpeed = 3.0f;

	// Use this for initialization
	void Start ()
    {
        //親の方へ向かって最初のDemonsが入ってるオブジェクトを取得
        parentDemon = this.gameObject.GetComponentInParent<Demons>();

        _particle = this.gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //ユニットが攻撃判定かつエフェクト再生されてなかった場合に呼び出し
        if(parentDemon.IsAttack && !_particle.isPlaying)
        {
            _particle.playbackSpeed = myplayBackSpeed;
            _particle.Play();
        }
        else if(!parentDemon.IsAttack)
        {
            _particle.Stop();
            _particle.time = 0;
        }	
	}
}
