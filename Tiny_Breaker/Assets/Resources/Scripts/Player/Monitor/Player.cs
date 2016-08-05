using UnityEngine;
using System.Collections.Generic;
using NCMB;

//大きい画面でのプレイヤークラス(スマホの情報の受け渡しとかプレイヤー番号とか送受信の解析とか)
public class Player : MonoBehaviour {

    //プレイヤーを識別する番号
    //誰の悪魔が魂の回収を行ったのかを判断するため必要だと思った
    public int playerID = 1;

    //スマホ側に送信するのに一時的にデータをためておく場所
    List<GrowPoint> spiritsData = new List<GrowPoint>();

    //仮//
    bool onecall = false;
    int counter = 6000;
    int typeNum = 0;

    string oldupdateDate = "";

    enum SET_TYPE
    {
        PUPU,
        POPO,
        PIPI
    }

    public static int playerStatusHP;
    public static int playerStatusATK;
    public static int playerStatusSPEED;

    [SerializeField, TooltipAttribute("悪魔")]
    private GameObject[] demon;
    public GameObject[] Demon
    {
        get { return demon; }
        set { demon = value; }
    }

    [SerializeField, TooltipAttribute("出撃位置")]
    private Vector3 spawnPosition = new Vector3(0, 1, -22);
    public Vector3 SpawnPosition { set { spawnPosition = value; } }
    //仮//


    void Start ()
    {
        for (int i = 0; i < demon.Length; i++)
        {
            //仮に何も設定されていなかったら空のゲームオブジェクトを入れる
            if (demon[i] == null)
                demon[i] = new GameObject();
        }
    }
	
	void Update ()
    {
        if (counter > 30)
        {
            counter = 0;
            
            //クエリを作成
            NCMBQuery<NCMBObject> demonStatus = new NCMBQuery<NCMBObject>("DemonData");

            //PlayerNoが記入されていないもの以外を取得
            demonStatus.WhereNotEqualTo("PlayerNo", "");

            //createDateを降順にしてリミットを1に制限することで最新のもののみ取得
            //demonStatus.OrderByDescending("createDate");
            //demonStatus.Limit = 1;

            //検索
            demonStatus.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
            //検索失敗時
            if (e != null)
                {
                    Debug.Log(e.ToString());
                    return;
                }
                else
                {
                    foreach (NCMBObject ncmbObj in objList)
                    {      
                        if (ncmbObj["Order"].ToString() == "Summon")
                        {
                            //ステータスを算出する
                            playerStatusHP = System.Convert.ToInt32(ncmbObj["HP"].ToString());
                            playerStatusATK = System.Convert.ToInt32(ncmbObj["ATK"].ToString());
                            playerStatusSPEED = System.Convert.ToInt32(ncmbObj["DEX"].ToString());

                            //適当な値を入れて重なることを避ける
                            Vector3 randVac = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

                            if(ncmbObj["Type"].ToString() == "PUPU")
                            {
                                typeNum = (int)SET_TYPE.PUPU;
                            }
                            else if (ncmbObj["Type"].ToString() == "POPO")
                            {
                                typeNum = (int)SET_TYPE.POPO;
                            }
                            else if (ncmbObj["Type"].ToString() == "PIPI")
                            {
                                typeNum = (int)SET_TYPE.PIPI;
                            }

                            //悪魔を出す
                            GameObject instaceObject = (GameObject)Instantiate(demon[typeNum],
                                                                            spawnPosition + demon[typeNum].transform.position + randVac,           //プレイヤーごとの出撃位置
                                                                            Quaternion.identity);
                            GameObject playerObject = GameObject.Find("Player");        //別の方法でプレイヤーを取得方法を考えたい
                            instaceObject.transform.SetParent(playerObject.transform, false);

                            ncmbObj["Order"] = "Summoned";

                            ncmbObj.SaveAsync();
                        }
                        else if (ncmbObj["Order"].ToString() == "Soldier")
                        {

                        }
                        else
                        {

                        }
                    }
                }
            });
        }

        counter++;

    }
    
    //魂リストへの追加
    public void AddSpiritList(GrowPoint spiritdata)
    {
        spiritsData.Add(spiritdata);
    }
}
