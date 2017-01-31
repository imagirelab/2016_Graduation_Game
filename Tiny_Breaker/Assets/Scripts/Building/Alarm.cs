using UnityEngine;
using StaticClass;

public class Alarm : MonoBehaviour
{
    [SerializeField]
    GameObject[] alarmObj = new GameObject[(int)Enum.Direction_TYPE.Num];

    [SerializeField]
    string targetTag = "";

    SphereCollider col;
    
    void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    void Update()
    {
        for (int i = 0; i < (int)Enum.Direction_TYPE.Num; i++)
        {
            GameObject nearDem = DemonDataBase.getInstance().GetNearestObject(targetTag, this.transform.position, (Enum.Direction_TYPE)i);
            GameObject nearSol = SolgierDataBase.getInstance().GetNearestObject(transform.gameObject.tag, this.transform.position, (Enum.Direction_TYPE)i);

            GameObject nearestObj = null;

            if (nearDem != null && nearSol == null)
                nearestObj = nearDem;
            if (nearDem == null && nearSol != null)
                nearestObj = nearSol;
            if (nearDem != null && nearSol != null)
                if (Vector3.Distance(this.transform.position, nearDem.transform.position) <
                    Vector3.Distance(this.transform.position, nearSol.transform.position))
                    nearestObj = nearDem;
                else
                    nearestObj = nearSol;
            

            if (nearestObj != null)
                if (Vector3.Distance(this.transform.position, nearestObj.transform.position) < col.radius)
                {
                    alarmObj[i].SetActive(true);
                }
                else
                    alarmObj[i].SetActive(false);
            else
                alarmObj[i].SetActive(false);
        }
    }
}