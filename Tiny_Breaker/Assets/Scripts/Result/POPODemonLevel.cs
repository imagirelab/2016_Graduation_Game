using UnityEngine;
using StaticClass;

public class POPODemonLevel : MonoBehaviour
{
    [SerializeField]
    int PlayerID = 0;

    [SerializeField]
    Sprite[] numbars = new Sprite[10];

    [SerializeField]
    SpriteRenderer ones = null;
    [SerializeField]
    SpriteRenderer tens = null;
    
    void Update()
    {
        int value = RoundDataBase.getInstance().POPOLevel[PlayerID];

        int tensNum = (Mathf.FloorToInt(value) % 100) / 10;
        int onesNum = Mathf.FloorToInt(value) % 10;

        if (tensNum < 10)
            tens.sprite = numbars[tensNum];
        if (onesNum < 10)
            ones.sprite = numbars[onesNum];
    }
}