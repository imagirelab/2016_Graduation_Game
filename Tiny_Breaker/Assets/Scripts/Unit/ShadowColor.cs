using UnityEngine;
using StaticClass;

public class ShadowColor : MonoBehaviour
{
    [SerializeField]
    Sprite[] shadow = new Sprite[GameRule.playerNum + 1];

    //対象
    public GameObject target;

    void Start()
    {
        switch (target.tag)
        {
            case "Player1":
                GetComponent<SpriteRenderer>().sprite = shadow[0];
                break;
            case "Player2":
                GetComponent<SpriteRenderer>().sprite = shadow[1];
                break;
            default:
                GetComponent<SpriteRenderer>().sprite = shadow[2];
                break;
        }
    }
}
