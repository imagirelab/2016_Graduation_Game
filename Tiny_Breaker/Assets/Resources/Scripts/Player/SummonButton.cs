//召喚ボタンを押したときの処理

using UnityEngine;

public class SummonButton : MonoBehaviour
{

    [SerializeField, TooltipAttribute("悪魔")]
    private GameObject demon;

    //成長値だけを記憶する変数(プレハブを参照すると他の悪魔に影響を与えるため)
    private DemonsGrowPointData growPoint;

    void Start()
    {
        //悪魔の初期値の設定
        demon.GetComponent<Demons>().growPoint.SetGrowPoint();
        //成長値だけを記憶する
        growPoint = new DemonsGrowPointData();
        growPoint.CurrentHP_GrowPoint = demon.GetComponent<Demons>().growPoint.GetHP_GrowPoint;
        growPoint.CurrentATK_GrowPoint = demon.GetComponent<Demons>().growPoint.GetATK_GrowPoint;
        growPoint.CurrentSPEED_GrowPoint = demon.GetComponent<Demons>().growPoint.GetSPEED_GrowPoint;
        growPoint.CurrentAtackTime_GrowPoint = demon.GetComponent<Demons>().growPoint.GetAtackTime_GrowPoint;
    }


    public void SetDemon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if(player.GetComponent<PlayerControl>().CurrentOrder == PlayerControl.Order.Catcher)
        {
            //魂をクリックしている状態でボタンに触れた時の処理
            if(player.GetComponent<PlayerControl>().FieldCommand.GetComponent<MouseControl>().ClickGameObject.tag == "Spirit")
            {
                growPoint.CurrentHP_GrowPoint
                    += player.GetComponent<SpiritCatcher>().GetClickSpirit.GetComponent<DemonsSpirits>().GrowPoint.CurrentHP_GrowPoint;
                growPoint.CurrentATK_GrowPoint
                    += player.GetComponent<SpiritCatcher>().GetClickSpirit.GetComponent<DemonsSpirits>().GrowPoint.CurrentATK_GrowPoint;
                growPoint.CurrentSPEED_GrowPoint
                    += player.GetComponent<SpiritCatcher>().GetClickSpirit.GetComponent<DemonsSpirits>().GrowPoint.CurrentSPEED_GrowPoint;
                growPoint.CurrentAtackTime_GrowPoint
                    += player.GetComponent<SpiritCatcher>().GetClickSpirit.GetComponent<DemonsSpirits>().GrowPoint.CurrentAtackTime_GrowPoint;
                
                Destroy(player.GetComponent<SpiritCatcher>().GetClickSpirit);
            }
            
        }

        player.GetComponent<SummonManager>().SettingDemonFlag = true;
        player.GetComponent<SummonManager>().SummonDemon = demon;
        player.GetComponent<SummonManager>().GrowPoint = growPoint;
    }
}
