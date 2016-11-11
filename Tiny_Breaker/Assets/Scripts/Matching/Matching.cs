using UnityEngine;
using System.Collections.Generic;
using NCMB;

public class Matching : MonoBehaviour
{
    public int receiveFlame = 60;

    public GameObject okObj;

    private int counter = 0;

    private int NoType = 0;

    bool Player1Ok;
    bool Player2Ok;

    // Use this for initialization
    void Start ()
    {
        counter = 0;
        Player1Ok = Player2Ok = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter++;

        if (counter > receiveFlame)
        {
            Receive();
            counter = 0;
        }

        if(Player1Ok == true && Player2Ok == true)
        {
            FadeManager.Instance.LoadLevel("MainScene", 1);
        }
	}

    //受信したときの処理
    void Receive()
    {
        //クエリを作成
        NCMBQuery<NCMBObject> playerMode = new NCMBQuery<NCMBObject>("PlayerData");

        //PlayerNoが0以外を取得
        playerMode.WhereNotEqualTo("PlayerNo", "0");

        //createDateを降順にしてリミットを1に制限することで最新のもののみ取得
        //demonStatus.OrderByDescending("createDate");
        //demonStatus.Limit = 1;

        //検索
        playerMode.FindAsync((List<NCMBObject> objList, NCMBException e) =>
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
                    if (ncmbObj["Mode"].ToString() == "matching")
                    {
                        NoType = System.Convert.ToInt32(ncmbObj["PlayerNo"]);

                        switch(NoType)
                        {
                            case 1:
                                Instantiate(okObj, new Vector3(2f,0f,0f), okObj.transform.rotation);
                                //記録を取ったらPCの準備が出来たことを連絡
                                ncmbObj["Mode"] = "PCOK";
                                ncmbObj.SaveAsync();
                                Player1Ok = true;
                                break;
                            case 2:
                                Instantiate(okObj, new Vector3(2f, -1.5f, 0f), okObj.transform.rotation);
                                //記録を取ったらPCの準備が出来たことを連絡
                                ncmbObj["Mode"] = "PCOK";
                                ncmbObj.SaveAsync();
                                Player2Ok = true;
                                break;
                            default:
                                break;

                        }
                    }
                }
            }
        });
    }
}
