//召喚ボタンを押したときの処理

using UnityEngine;

public class SummonButton : MonoBehaviour
{

    [SerializeField, TooltipAttribute("悪魔")]
    private GameObject demon;
    
    public void SetDemon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.GetComponent<SummonManager>().SettingDemonFlag)
        {
            Debug.Log("Unlock Demon");
            player.GetComponent<SummonManager>().SettingDemonFlag = false;

            player.GetComponent<SummonManager>().SummonDemon = null;
        }
        else
        {
            Debug.Log("Set Demon");
            player.GetComponent<SummonManager>().SettingDemonFlag = true;

            player.GetComponent<SummonManager>().SummonDemon = demon;
        }
    }
}
