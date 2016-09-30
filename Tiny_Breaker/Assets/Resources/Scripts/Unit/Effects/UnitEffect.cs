using UnityEngine;
using System.Collections;

public class UnitEffect : MonoBehaviour
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
    //１個上のハウスを取得するための置物
    private House parentHouse;
    //１個上の拠点を取得するための置物
    private DefenseBase parentBase;

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
            //親の方へ向かって最初のSoldierが入ってるオブジェクトを取得
            parentSoldier = this.gameObject.GetComponentInParent<Soldier>();
        }
        else if(unitType == TYPE.House)
        {
            //親の方へ向かって最初のHouseが入ってるオブジェクトを取得
            parentHouse = this.gameObject.GetComponentInParent<House>();
        }
        else if (unitType == TYPE.Base)
        {
            //親の方へ向かって最初のHouseが入ってるオブジェクトを取得
            parentBase = this.gameObject.GetComponentInParent<DefenseBase>();
        }

        _particle = this.gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //それぞれコンポーネントの取得先が違うので別々に
        if (unitType == TYPE.Demon)
        {
            if (parentDemon.IsDamage && !_particle.isPlaying)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }
        }
        else if (unitType == TYPE.Soldier)
        {
            if (parentSoldier.IsDamage && !_particle.isPlaying)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }
        }
        else if (unitType == TYPE.House)
        {
            if (parentHouse.IsDamage && !_particle.isPlaying)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }
        }
        else if (unitType == TYPE.Base)
        {
            if (parentBase.IsDamage && !_particle.isPlaying)
            {
                _particle.time = 0;
                _particle.playbackSpeed = myplayBackSpeed;
                _particle.Play();
            }
        }
    }
}
