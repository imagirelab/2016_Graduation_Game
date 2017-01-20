using UnityEngine;

public class AttackLocus : MonoBehaviour
{
    public Unit unit;
    public GameObject lucusPre;
    
	void Update ()
	{
        if (unit.state == Enum.State.Attack)
        {
            lucusPre.SetActive(true);
        }
        else
        {
            lucusPre.SetActive(false);
        }

	}
}