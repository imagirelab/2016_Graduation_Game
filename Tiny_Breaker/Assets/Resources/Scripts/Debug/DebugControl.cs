using UnityEngine;
using System.Collections.Generic;
using StaticClass;

public class DebugControl : MonoBehaviour
{
    [SerializeField, TooltipAttribute("デバック表示するかどうか")]
    bool IsDebug = false;
    
    [SerializeField]
    List<GameObject> DebugObject = new List<GameObject>();
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject POPO;
    [SerializeField]
    GameObject PUPU;
    [SerializeField]
    GameObject PIPI;

    void Start()
    {
        if (player == null)
            player = new GameObject();
        
        if (POPO == null)
            POPO = new GameObject();
        if (PUPU == null)
            PUPU = new GameObject();
        if (PIPI == null)
            PIPI = new GameObject();

        GameRule.getInstance().debugFlag = IsDebug;
        
        SetDebugActive(GameRule.getInstance().debugFlag);
    }

    void Update()
    {
        //デバッグ表示の切り替え
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            GameRule.getInstance().debugFlag = !GameRule.getInstance().debugFlag;
            SetDebugActive(GameRule.getInstance().debugFlag);
        }

        //Player1の方の操作
        if (player.GetComponent<Player>() != null)
        {
            Player playerComp = player.GetComponent<Player>();

            //パワーアップ
            if (Input.GetKeyDown(KeyCode.Q))
                playerComp.DebugPowerUP(PUPU);
            if (Input.GetKeyDown(KeyCode.A))
                playerComp.DebugPowerUP(POPO);
            if (Input.GetKeyDown(KeyCode.Z))
                playerComp.DebugPowerUP(PIPI);

            //PIPI召喚
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerComp.ChangeRoot(2);
                playerComp.DebugSummon(PIPI);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                playerComp.ChangeRoot(1);
                playerComp.DebugSummon(PIPI);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                playerComp.ChangeRoot(0);
                playerComp.DebugSummon(PIPI);
            }

            //PUPU召喚
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerComp.ChangeRoot(2);
                playerComp.DebugSummon(PUPU);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                playerComp.ChangeRoot(1);
                playerComp.DebugSummon(PUPU);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerComp.ChangeRoot(0);
                playerComp.DebugSummon(PUPU);
            }

            //POPO召喚
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerComp.ChangeRoot(2);
                playerComp.DebugSummon(POPO);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerComp.ChangeRoot(1);
                playerComp.DebugSummon(POPO);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                playerComp.ChangeRoot(0);
                playerComp.DebugSummon(POPO);
            }
        }
    }

    void SetDebugActive(bool flag)
    {
        foreach(GameObject e in DebugObject)
            e.SetActive(flag);
    }
}
