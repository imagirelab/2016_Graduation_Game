//プレイヤーが操作する画面の根底にあるクラス

using UnityEngine;
using System.Collections.Generic;

public class PlayerScreen : MonoBehaviour {

    private int playerID = 0;
    public int PlayerID { get { return playerID; } set { playerID = value; } }

    //スマホ側に受信するのに一時的にデータをためておく場所
    List<GrowPoint> spiritsData = new List<GrowPoint>();

    public GameObject POPOIcon;
    public GameObject PUPUIcon;
    public GameObject PIPIIcon;

    void Update () {
        //リストに魂のデータが入っていたら処理
        if (spiritsData.Count > 0)
        {
            for (int i = 0; i < spiritsData.Count; i++)
            {
                switch(spiritsData[i].GetDemonType)
                {
                    case GrowPoint.Type.POPO:
                        CreateSpiritIcon(POPOIcon, i);
                        break;
                    case GrowPoint.Type.PUPU:
                        CreateSpiritIcon(PUPUIcon, i);
                        break;
                    case GrowPoint.Type.PIPI:
                        CreateSpiritIcon(PIPIIcon, i);
                        break;
                }
            }

            //リストの送ったら中身を消す
            spiritsData.Clear();
        }
    }

    //種類ごとにアイコンを作るところ
    void CreateSpiritIcon(GameObject gameObject, int index)
    {
        Transform spiritBox = this.transform.FindChild("SpiritBox");
        int intoboxCount = spiritBox.childCount;

        //種類によって変えるところ
        GameObject spiritIcon = (GameObject)Instantiate(gameObject,
            new Vector3(gameObject.transform.position.x,
                        gameObject.transform.position.y - 25.0f * (index + intoboxCount),
                        gameObject.transform.position.z),
            Quaternion.identity);
        spiritIcon.transform.SetParent(spiritBox, false);
        spiritIcon.GetComponent<IconSpirit>().GrowPoint = spiritsData[index];
    }

    //外部から魂の情報をもらうところ
    public void AddSpiritList(GrowPoint spiritdata)
    {
        spiritsData.Add(spiritdata);
    }
}
