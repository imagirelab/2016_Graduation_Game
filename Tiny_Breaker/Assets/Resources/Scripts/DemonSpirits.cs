using UnityEngine;
using System.Collections;

public class DemonSpirits : MonoBehaviour
{
    //プレイヤーの仮ステータス
    public int spiritsHP = 0;
    public int HPpro { get { return spiritsHP; } set { spiritsHP = value; } }
    public int spiritsATK = 0;
    public int ATKpro { get { return spiritsATK; } set { spiritsATK = value; } }
    public int spiritsSPEED = 0;
    public int SPEEDpro { get { return spiritsSPEED; } set { spiritsSPEED = value; } }

    public GameObject _Demon;
    
    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(StaticVariables.catcherFlag)
        {
            _Demon.GetComponent<Demons>().HPpro += spiritsHP;
            _Demon.GetComponent<Demons>().ATKpro += spiritsATK;
            _Demon.GetComponent<Demons>().SPEEDpro += spiritsSPEED;

            Instantiate(_Demon);
            StaticVariables.catcherFlag = false;
            Destroy(this.gameObject);
        }
	}
}
