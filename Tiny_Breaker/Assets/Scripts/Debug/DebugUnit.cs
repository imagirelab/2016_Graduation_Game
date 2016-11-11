using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugUnit : MonoBehaviour
{
    //プレイヤーが持っている悪魔のデータを使う
    [SerializeField]
    Player player = null;
    int demonsPage = 0;

    int[] level = new int[(int)Enum.Demon_TYPE.Num];
    Status[] demonsStatus = new Status[(int)Enum.Demon_TYPE.Num];

    //[SerializeField]
    //GameObject Shield;
    //[SerializeField]
    //GameObject Ax;
    //[SerializeField]
    //GameObject Gun;
    //List<GameObject> soldiers = new List<GameObject>();
    //int soldiersPage = 0;

    void Start()
    {
        demonsPage = 0;

        //悪魔のレベル
        level = player.DemonsLevel;

        //プレイヤーのステータス先を取得
        demonsStatus = player.DemonsStatus;
        
        //if (Shield == null)
        //    Shield = new GameObject(this.ToString() + "Shield");
        //if (Ax == null)
        //    Ax = new GameObject(this.ToString() + "Ax");
        //if (Gun == null)
        //    Gun = new GameObject(this.ToString() + "Gun");

        //soldiers.Add(Shield);
        //soldiers.Add(Ax);
        //soldiers.Add(Gun);

        //soldiersPage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのステータス先を取得
        demonsStatus = player.DemonsStatus;

        this.GetComponent<Text>().text = "Demon:" + player.Demons[demonsPage].name + "\n" +
                                         "LEVEL:" + level[demonsPage] + "\n" +
                                         "HP:" + demonsStatus[demonsPage].CurrentHP + "\n" +
                                         "ATK:" + demonsStatus[demonsPage].CurrentATK + "\n" +
                                         "SPD:" + demonsStatus[demonsPage].CurrentSPEED.ToString("f2") + "\n" +
                                         "ATK_T:" + demonsStatus[demonsPage].CurrentAtackTime.ToString("f2");
                                         //"\n" +
                                         //"Enemy:" + soldiers[soldiersPage].name + "\n" +
                                         //"HP:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentHP + "\n" +
                                         //"ATK:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentATK + "\n" +
                                         //"SPD:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentSPEED.ToString("f2") + "\n" +
                                         //"ATKTime:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentAtackTime.ToString("f2");

        //ページ送り
        if (Input.GetKeyDown(KeyCode.Keypad9))
            AddDemonsPage(1);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            AddDemonsPage(-1);
        //if (Input.GetKeyDown(KeyCode.Keypad6))
        //    AddSoldiersPage(1);
        //if (Input.GetKeyDown(KeyCode.Keypad4))
        //    AddSoldiersPage(-1);
    }

    //進めたいページ数 戻したいときはマイナスを入れる
    public void AddDemonsPage(int add)
    {
        demonsPage += add;

        if (demonsPage >= demonsStatus.Length)
        {
            demonsPage %= demonsStatus.Length;
        }
        if (demonsPage < 0)
        {
            demonsPage %= demonsStatus.Length;
            demonsPage += demonsStatus.Length;
        }
    }

    ////進めたいページ数 戻したいときはマイナスを入れる
    //public void AddSoldiersPage(int add)
    //{
    //    soldiersPage += add;

    //    if (soldiersPage >= soldiers.Count)
    //    {
    //        soldiersPage %= soldiers.Count;
    //    }
    //    if (soldiersPage < 0)
    //    {
    //        soldiersPage %= soldiers.Count;
    //        soldiersPage += soldiers.Count;
    //    }
    //}
}