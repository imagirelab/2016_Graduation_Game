using UnityEngine;
using System.Collections;

public class SummonManager : MonoBehaviour {

    private bool settingDemonFlag;
    private GameObject summonDemon;

    public bool SettingDemonFlag {
        get { return settingDemonFlag; }
        set { settingDemonFlag = value; }
    }
    public GameObject SummonDemon {
        get { return summonDemon; }
        set { summonDemon = value; }
    }

    // Use this for initialization
    void Start () {
        settingDemonFlag = false;
        summonDemon = null;
    }
	
	// Update is called once per frame
	void Update () {
	    if(this.GetComponent<PlayerControl>().CurrentOrder == PlayerControl.Order.Summon)
            if (this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().IsClick &&
                this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickGameObject.tag == "Ground")
                SummonOrder();
    }


    // 悪魔を召喚するときの処理
    void SummonOrder()
    {
        Instantiate(this.GetComponent<SummonManager>().SummonDemon,
                    this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickPosition,
                    Quaternion.identity);
        this.GetComponent<SummonManager>().SettingDemonFlag = false;
    }
}
