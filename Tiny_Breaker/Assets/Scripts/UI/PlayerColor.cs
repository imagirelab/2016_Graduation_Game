using UnityEngine;
using UnityEngine.UI;
using StaticClass;

public class PlayerColor : MonoBehaviour
{
    //枠画像
    [SerializeField]
    Image frame;

    //差し替える枠画像
    [SerializeField]
    Sprite[] frameSprites = new Sprite[GameRule.playerNum + 1];

    //体力になる画像
    [SerializeField]
    Image HP;

    //差し替える体力になる画像
    [SerializeField]
    Sprite[] HPSprites = new Sprite[GameRule.playerNum + 1];

    //表示する色のプレイヤーID
    int playerID = 0;

    void Start()
    {
        playerID = 0;

        //根底がプレイヤーだったらplayerIDの取得
        GameObject rootObject = transform.root.gameObject;
        if (rootObject.GetComponent<Player>() != null)
            playerID = rootObject.GetComponent<Player>().playerID;

        if (frame == null)
            frame = null;
        if (HP == null)
            HP = null;
    }

    void Update()
    {
        //根底がスポナーの時playerIDの取得
        GameObject rootObject = transform.root.gameObject;
        if (rootObject.GetComponent<Spawner>() != null)
            playerID = rootObject.GetComponent<Spawner>().CurrentPlayerID;

        //バーとスライダーの色変化
        frame.sprite = frameSprites[playerID];
        HP.sprite = HPSprites[playerID];
    }
}
