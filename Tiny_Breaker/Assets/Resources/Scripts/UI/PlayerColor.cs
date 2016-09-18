using UnityEngine;
using UnityEngine.UI;
using StaticClass;

public class PlayerColor : MonoBehaviour
{

    [SerializeField]
    SpriteRenderer barSprite = new SpriteRenderer();

    [SerializeField]
    Image fillImage;

    [SerializeField]
    Sprite[] sprites = new Sprite[GameRule.getInstance().playerNum + 1];

    [SerializeField]
    Color[] colors = new Color[GameRule.getInstance().playerNum + 1];

    //表示する色のプレイヤーID
    int playerID = 0;

    void Start()
    {
        playerID = 0;

        //根底がプレイヤーだったらplayerIDの取得
        GameObject rootObject = transform.root.gameObject;
        if (rootObject.GetComponent<Player>() != null)
            playerID = rootObject.GetComponent<Player>().playerID;

        if (fillImage == null)
        {
            gameObject.AddComponent<Image>();
            fillImage = GetComponent<Image>();
        }
    }

    void Update()
    {
        //根底がスポナーの時playerIDの取得
        GameObject rootObject = transform.root.gameObject;
        if (rootObject.GetComponent<Spawner>() != null)
            playerID = rootObject.GetComponent<Spawner>().CurrentPlayerID;

        //バーとスライダーの色変化
        barSprite.sprite = sprites[playerID];
        fillImage.color = colors[playerID];
    }
}
