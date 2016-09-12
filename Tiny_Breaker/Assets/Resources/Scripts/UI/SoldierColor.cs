using UnityEngine;
using StaticClass;

public class SoldierColor : MonoBehaviour
{
    [SerializeField]
    TextMesh text;

    [SerializeField]
    Color[] colors = new Color[GameRule.getInstance().playerNum + 1];

    void Start()
    {
        if (text == null)
        {
            gameObject.AddComponent<TextMesh>();
            text = GetComponent<TextMesh>();
        }
    }

    void Update()
    {
        //根底がスポナーの時playerIDの取得
        GameObject rootObject = transform.root.gameObject;
        if (rootObject.GetComponent<Spawner>() != null)
        {
            int playerID = rootObject.GetComponent<Spawner>().CurrentPlayerID;
            text.color = colors[playerID];
        }
    }
}
