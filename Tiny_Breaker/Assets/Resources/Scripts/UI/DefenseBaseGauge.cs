using UnityEngine;

public class DefenseBaseGauge : MonoBehaviour
{
    [SerializeField]
    DefenseBase player;

    RectTransform PlayerHP;

    void Start()
    {
        if (GetComponent<RectTransform>())
            PlayerHP = GetComponent<RectTransform>();
        else
            PlayerHP = new RectTransform();

        if (player == null)
            player = new DefenseBase();
    }

    void Update()
    {
        float HPRate = (float)player.HPpro / (float)player.GetHP;

        PlayerHP.localScale = new Vector3(HPRate, PlayerHP.localScale.y, PlayerHP.localScale.z);
    }
}