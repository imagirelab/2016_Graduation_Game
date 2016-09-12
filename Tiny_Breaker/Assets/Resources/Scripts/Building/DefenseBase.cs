using UnityEngine;
using UnityEngine.UI;

public class DefenseBase : MonoBehaviour
{
    //HP
    [SerializeField, TooltipAttribute("体力")]
    private int HP = 1000;
    public int HPpro { get { return HP; } set { HP = value; } }

    //HPのUI
    private GameObject HP_UI;

    //表示するテキスト
    [SerializeField]
    Text Text = null;

    void Start()
    {
        HP_UI = transform.FindChild("HP").gameObject;
    }

    void Update()
    {
        HP_UI.GetComponent<TextMesh>().text = "HP: " + HP.ToString();

        if (HP <= 0)
        {
            //文字表示
            Text.enabled = true;
            HP = 0;
            //Destroy(gameObject);
        }

    }
}
