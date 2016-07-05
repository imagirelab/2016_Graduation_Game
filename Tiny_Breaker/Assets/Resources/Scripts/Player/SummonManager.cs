using UnityEngine;
using System.Collections;

public class SummonManager : MonoBehaviour
{
    [SerializeField, Range(0, 9), TooltipAttribute("ボタンの数")]
    private int ButtonCount = 0;
    
    private bool settingDemonFlag;          //出撃準備中かどうかのフラグ
    private GameObject summonDemon;         //出す予定の悪魔
    private DemonsGrowPointData growPoint;  //出す予定の悪魔の成長値

    public bool SettingDemonFlag {
        get { return settingDemonFlag; }
        set { settingDemonFlag = value; }
    }
    public GameObject SummonDemon {
        get { return summonDemon; }
        set { summonDemon = value; }
    }
    public DemonsGrowPointData GrowPoint
    {
        get { return growPoint; }
        set { growPoint = value; }
    }

    // Use this for initialization
    void Start ()
    {
        settingDemonFlag = false;
        summonDemon = null;

        //ボタンの配置を決める
        GameObject PIPIButton = (GameObject)Resources.Load("Prefabs/Button/PIPI_Bar");
        GameObject POPOButton = (GameObject)Resources.Load("Prefabs/Button/POPO_Bar");
        GameObject PUPUButton = (GameObject)Resources.Load("Prefabs/Button/PUPU_Bar");
        GameObject canvas = GameObject.Find("Canvas");

        for (int i = 0; i < ButtonCount; i++)
        {
            if (i % 3 == 0)
                InstantianteButton(canvas, PIPIButton, i);
            if(i % 3 == 1)
                InstantianteButton(canvas, POPOButton, i);
            if (i % 3 == 2)
                InstantianteButton(canvas, PUPUButton, i);
        }
    }

    void InstantianteButton(GameObject canvas, GameObject gameObject, int i)
    {
        GameObject button = (GameObject)Instantiate(gameObject,
                                                    new Vector3(gameObject.transform.position.x, gameObject.transform.position.y * (i + 1), gameObject.transform.position.z),
                                                    Quaternion.identity);
        button.transform.SetParent(canvas.transform, false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //条件
        //プレイヤーが召喚指示を出していて、
        //マウスがクリックされた時で、
        //地面(召喚できるスペース)を選んでいたら、
        //出せる
        if (this.GetComponent<PlayerControl>().CurrentOrder == PlayerControl.Order.Summon)
            if (this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().IsClick &&
                this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickGameObject.tag == "Ground")
                SummonOrder();
    }
    
    // 悪魔を召喚するときの処理
    void SummonOrder()
    {
        GameObject summonDemonClone = (GameObject)Instantiate(summonDemon,
                                                              this.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickPosition + summonDemon.transform.position,
                                                              Quaternion.identity);
        switch (summonDemon.name)
        {
            case "PIPI":
                SetDemonsStatus(summonDemonClone);
                break;
            case "POPO":
                SetDemonsStatus(summonDemonClone);
                break;
            case "PUPU":
                SetDemonsStatus(summonDemonClone);
                break;
            default:
                break;
        }
        //召喚が終わったらフラグを戻す
        this.GetComponent<SummonManager>().SettingDemonFlag = false;
    }

    void SetDemonsStatus(GameObject demon)
    {
        //初期ステータス(プロパティからの設定情報)
        demon.GetComponent<Demons>().status.SetStutas();
        demon.GetComponent<Demons>().growPoint.SetGrowPoint();
        
        for (int i = 0; i < this.growPoint.CurrentHP_GrowPoint - demon.GetComponent<Demons>().growPoint.GetHP_GrowPoint; i++)
            demon.GetComponent<Demons>().status.CurrentHP += (int)(demon.GetComponent<Demons>().status.GetHP * 0.5f);
        for (int i = 0; i < this.growPoint.CurrentATK_GrowPoint - demon.GetComponent<Demons>().growPoint.GetATK_GrowPoint; i++)
            demon.GetComponent<Demons>().status.CurrentATK += (int)(demon.GetComponent<Demons>().status.GetATK * 0.5f);
        //for (int i = 0; i < summonDemon.GetComponent<Demons>().growPoint.SPEED_GrowPoint - defaultDemon.growPoint.SPEED_GrowPoint; i++)
        //    summonDemon.GetComponent<Demons>().status.SPEED += (int)(defaultDemon.status.SPEED * 0.5f);
        //for (int i = 0; i < summonDemon.GetComponent<Demons>().growPoint.AtackTime_GrowPoint - defaultDemon.growPoint.AtackTime_GrowPoint; i++)
        //    summonDemon.GetComponent<Demons>().status.AtackTime += (int)(defaultDemon.status.AtackTime * 0.5f);

        //成長値の代入
        demon.GetComponent<Demons>().growPoint = this.growPoint;

        Debug.Log("GrowPoint HP:" + growPoint.CurrentHP_GrowPoint + " ATK:" + growPoint.CurrentATK_GrowPoint + " SPEED:" + growPoint.CurrentSPEED_GrowPoint);
    }
}
