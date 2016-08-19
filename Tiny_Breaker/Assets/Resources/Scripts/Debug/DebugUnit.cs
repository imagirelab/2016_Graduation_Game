using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugUnit : MonoBehaviour
{

    [SerializeField]
    GameObject POPO;
    [SerializeField]
    GameObject PUPU;
    [SerializeField]
    GameObject PIPI;
    List<GameObject> demons = new List<GameObject>();
    int demonsPage = 0;

    [SerializeField]
    GameObject Shield;
    [SerializeField]
    GameObject Ax;
    [SerializeField]
    GameObject Gun;
    List<GameObject> soldiers = new List<GameObject>();
    int soldiersPage = 0;

    // Use this for initialization
    void Start()
    {
        if (POPO == null)
            POPO = new GameObject(this.ToString() + "POPO");
        if (PUPU == null)
            PUPU = new GameObject(this.ToString() + "PUPU");
        if (PIPI == null)
            PIPI = new GameObject(this.ToString() + "PIPI");

        demons.Add(POPO);
        demons.Add(PUPU);
        demons.Add(PIPI);

        demonsPage = 0;

        if (Shield == null)
            Shield = new GameObject(this.ToString() + "Shield");
        if (Ax == null)
            Ax = new GameObject(this.ToString() + "Ax");
        if (Gun == null)
            Gun = new GameObject(this.ToString() + "Gun");

        soldiers.Add(Shield);
        soldiers.Add(Ax);
        soldiers.Add(Gun);

        soldiersPage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Text>().text = "Demon:" + demons[demonsPage].name + "\n" +
                                         "HP:" + demons[demonsPage].GetComponent<Unit>().status.CurrentHP + "\t\t\t   G_HP:" + demons[demonsPage].GetComponent<Demons>().GrowPoint.CurrentHP_GrowPoint + "\n" +
                                         "ATK:" + demons[demonsPage].GetComponent<Unit>().status.CurrentATK + "\t\t\t   G_ATK:" + demons[demonsPage].GetComponent<Demons>().GrowPoint.CurrentATK_GrowPoint + "\n" +
                                         "SPD:" + demons[demonsPage].GetComponent<Unit>().status.CurrentSPEED.ToString("f2") + "\t\t   G_SPD:" + demons[demonsPage].GetComponent<Demons>().GrowPoint.CurrentSPEED_GrowPoint + "\n" +
                                         "ATKTime:" + demons[demonsPage].GetComponent<Unit>().status.CurrentAtackTime.ToString("f2") + "  G_ATKTime:" + demons[demonsPage].GetComponent<Demons>().GrowPoint.CurrentAtackTime_GrowPoint + "\n" +
                                         "\n" +
                                         "Enemy:" + soldiers[soldiersPage].name + "\n" +
                                         "HP:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentHP + "\n" +
                                         "ATK:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentATK + "\n" +
                                         "SPD:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentSPEED.ToString("f2") + "\n" +
                                         "ATKTime:" + soldiers[soldiersPage].GetComponent<Unit>().status.CurrentAtackTime.ToString("f2");
    }

    //進めたいページ数 戻したいときはマイナスを入れる
    public void AddDemonsPage(int add)
    {
        demonsPage += add;

        if (demonsPage >= demons.Count)
        {
            demonsPage %= demons.Count;
        }
        if (demonsPage < 0)
        {
            demonsPage %= demons.Count;
            demonsPage += demons.Count;
        }
    }

    //進めたいページ数 戻したいときはマイナスを入れる
    public void AddSoldiersPage(int add)
    {
        soldiersPage += add;

        if (soldiersPage >= soldiers.Count)
        {
            soldiersPage %= soldiers.Count;
        }
        if (soldiersPage < 0)
        {
            soldiersPage %= soldiers.Count;
            soldiersPage += soldiers.Count;
        }
    }
}