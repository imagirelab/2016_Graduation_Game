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
    Player player1 = null;
    [SerializeField]
    Player player2 = null;

    void Start()
    {
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

        #region Player1の方の操作

        if (player1 != null)
        {
            Player player = player1;

            //必殺技
            if (Input.GetKeyDown(KeyCode.LeftShift))
                player.Deathblow();
            if (Input.GetKeyDown(KeyCode.LeftControl))
                player.DebugDeathblow();

            //パワーアップ
            if (Input.GetKeyDown(KeyCode.Q))
                player.DebugPowerUP((int)Enum.Demon_TYPE.PUPU);
            if (Input.GetKeyDown(KeyCode.A))
                player.DebugPowerUP((int)Enum.Demon_TYPE.POPO);
            if (Input.GetKeyDown(KeyCode.Z))
                player.DebugPowerUP((int)Enum.Demon_TYPE.PIPI);

            //PIPI召喚
            if (Input.GetKeyDown(KeyCode.W))
                Summon(player, Enum.Direction_TYPE.Top, Enum.Demon_TYPE.PIPI);
            if (Input.GetKeyDown(KeyCode.S))
                Summon(player, Enum.Direction_TYPE.Middle, Enum.Demon_TYPE.PIPI);
            if (Input.GetKeyDown(KeyCode.X))
                Summon(player, Enum.Direction_TYPE.Bottom, Enum.Demon_TYPE.PIPI);

            //PUPU召喚
            if (Input.GetKeyDown(KeyCode.E))
                Summon(player, Enum.Direction_TYPE.Top, Enum.Demon_TYPE.PUPU);
            if (Input.GetKeyDown(KeyCode.D))
                Summon(player, Enum.Direction_TYPE.Middle, Enum.Demon_TYPE.PUPU);
            if (Input.GetKeyDown(KeyCode.C))
                Summon(player, Enum.Direction_TYPE.Bottom, Enum.Demon_TYPE.PUPU);

            //POPO召喚
            if (Input.GetKeyDown(KeyCode.R))
                Summon(player, Enum.Direction_TYPE.Top, Enum.Demon_TYPE.POPO);
            if (Input.GetKeyDown(KeyCode.F))
                Summon(player, Enum.Direction_TYPE.Middle, Enum.Demon_TYPE.POPO);
            if (Input.GetKeyDown(KeyCode.V))
                Summon(player, Enum.Direction_TYPE.Bottom, Enum.Demon_TYPE.POPO);
        }

        #endregion

        #region Player2の方の操作

        if (player2 != null)
        {
            Player player = player2;

            //必殺技
            if (Input.GetKeyDown(KeyCode.RightShift))
                player.Deathblow();
            if (Input.GetKeyDown(KeyCode.RightControl))
                player.DebugDeathblow();

            //パワーアップ
            if (Input.GetKeyDown(KeyCode.LeftBracket))
                player.DebugPowerUP((int)Enum.Demon_TYPE.PUPU);
            if (Input.GetKeyDown(KeyCode.Colon))
                player.DebugPowerUP((int)Enum.Demon_TYPE.POPO);
            if (Input.GetKeyDown(KeyCode.Slash))
                player.DebugPowerUP((int)Enum.Demon_TYPE.PIPI);

            //PIPI召喚
            if (Input.GetKeyDown(KeyCode.P))
                Summon(player, Enum.Direction_TYPE.Top, Enum.Demon_TYPE.PIPI);
            if (Input.GetKeyDown(KeyCode.L))
                Summon(player, Enum.Direction_TYPE.Middle, Enum.Demon_TYPE.PIPI);
            if (Input.GetKeyDown(KeyCode.Comma))
                Summon(player, Enum.Direction_TYPE.Bottom, Enum.Demon_TYPE.PIPI);

            //PUPU召喚
            if (Input.GetKeyDown(KeyCode.O))
                Summon(player, Enum.Direction_TYPE.Top, Enum.Demon_TYPE.PUPU);
            if (Input.GetKeyDown(KeyCode.K))
                Summon(player, Enum.Direction_TYPE.Middle, Enum.Demon_TYPE.PUPU);
            if (Input.GetKeyDown(KeyCode.M))
                Summon(player, Enum.Direction_TYPE.Bottom, Enum.Demon_TYPE.PUPU);

            //POPO召喚
            if (Input.GetKeyDown(KeyCode.I))
                Summon(player, Enum.Direction_TYPE.Top, Enum.Demon_TYPE.POPO);
            if (Input.GetKeyDown(KeyCode.J))
                Summon(player, Enum.Direction_TYPE.Middle, Enum.Demon_TYPE.POPO);
            if (Input.GetKeyDown(KeyCode.N))
                Summon(player, Enum.Direction_TYPE.Bottom, Enum.Demon_TYPE.POPO);
        }

        #endregion
    }

    void Summon(Player player, Enum.Direction_TYPE rootNum, Enum.Demon_TYPE type)
    {
        player.ChangeRoot((int)rootNum);
        player.DebugSummon((int)type);
    }

    void SetDebugActive(bool flag)
    {
        foreach (GameObject e in DebugObject)
            e.SetActive(flag);
    }
}
