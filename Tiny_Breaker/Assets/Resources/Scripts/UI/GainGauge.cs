using UnityEngine;

public class GainGauge : MonoBehaviour
{
    House house;

    //プレイヤー１側の画像
    [SerializeField]
    RectTransform Player1Color = new RectTransform();

    //プレイヤー２側の画像
    [SerializeField]
    RectTransform Player2Color = new RectTransform();

    void Start ()
    {
        if (transform.root.gameObject.GetComponent<House>())
            house = transform.root.gameObject.GetComponent<House>();
        else
            house = new House();
    }
	
	void Update ()
	{
        float HPRate = (float)(house.HPpro + house.GetHP) / (float)(house.GetHP * 2.0f);
        if (HPRate >= 1.0f)
            HPRate = 1.0f;
        if(HPRate <= 0.0f)
            HPRate = 0.0f;

        //プレイヤー１側
        Player1Color.localScale = new Vector3(HPRate, Player1Color.localScale.y, Player1Color.localScale.z);
        //プレイヤー２側
        Player2Color.localScale = new Vector3(1.0f - HPRate, Player2Color.localScale.y, Player2Color.localScale.z);
    }
}