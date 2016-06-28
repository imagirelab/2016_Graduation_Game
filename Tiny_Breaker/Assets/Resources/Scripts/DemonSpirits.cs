using UnityEngine;
using System.Collections;

public class DemonSpirits : MonoBehaviour
{
    //プレイヤーの仮ステータス
    private DemonStatus status;
    private string demonName;

    
    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(StaticVariables.catcherFlag)
        {
            GameObject prefab = (GameObject)Resources.Load("Prefabs/Unit/" + demonName);
            GameObject demon = (GameObject)Instantiate(prefab);
            demon.GetComponent<Demons>().AddStatus(status);
            StaticVariables.catcherFlag = false;
            Destroy(this.gameObject);
        }
	}

    public static void InstanceSpirit(Object spiritPrefab, GameObject demon)
    {
        GameObject spirit = (GameObject)Instantiate(spiritPrefab, demon.transform.position, demon.transform.rotation);
        spirit.GetComponent<DemonSpirits>().status = demon.GetComponent<Demons>().status;
        spirit.GetComponent<DemonSpirits>().demonName = demon.name.Replace("(Clone)", "");
    }
}
