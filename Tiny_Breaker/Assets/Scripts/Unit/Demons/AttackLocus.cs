using UnityEngine;

public class AttackLocus : MonoBehaviour
{
    public Unit unit;
    public GameObject normalAttackEffect;
    public GameObject powerUpAttackEffect;
    
    void Update ()
	{
        //進化前の攻撃エフェクト
        if (unit.level < unit.powerupLevel)
        {
            if (powerUpAttackEffect != null)
                powerUpAttackEffect.SetActive(false);

            if (normalAttackEffect != null)
                if (unit.state == Enum.State.Attack)
                    normalAttackEffect.SetActive(true);
                else
                    normalAttackEffect.SetActive(false);
        }

        //進化後の攻撃エフェクト
        if (unit.level >= unit.powerupLevel)
        {
            if (normalAttackEffect != null)
                normalAttackEffect.SetActive(false);

            if (powerUpAttackEffect != null)
                if (unit.state == Enum.State.Attack)
                    powerUpAttackEffect.SetActive(true);
                else
                    powerUpAttackEffect.SetActive(false);
        }
    }
}