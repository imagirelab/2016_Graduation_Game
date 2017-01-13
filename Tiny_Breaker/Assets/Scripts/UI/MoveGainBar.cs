using UnityEngine;

public class MoveGainBar : MonoBehaviour
{
    House house;

    //バーの画像
    [SerializeField]
    RectTransform bar = new RectTransform();

    [SerializeField]
    float min = -230;

    [SerializeField]
    float max = 230;

    void Start ()
    {
        house = transform.parent.gameObject.GetComponent<House>();
    }
	
	void Update ()
	{
        float HPRate = (float)(house.HPpro + house.GetHP) / (float)(house.GetHP * 2.0f);
        if (HPRate > 1.0f)
            HPRate = 1.0f;
        if (HPRate < 0.0f)
            HPRate = 0.0f;
        float sub = max - min;

        //バーの移動
        bar.localPosition = new Vector3((sub * HPRate) + min, bar.localPosition.y, bar.localPosition.z);
    }
}