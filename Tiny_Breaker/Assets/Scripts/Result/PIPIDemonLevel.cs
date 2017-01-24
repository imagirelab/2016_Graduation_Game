using UnityEngine;
using StaticClass;

public class PIPIDemonLevel : MonoBehaviour
{
    [SerializeField]
    int PlayerID = 0;

    [SerializeField]
    Sprite[] numbars = new Sprite[10];

    [SerializeField]
    SpriteRenderer ones = null;
    [SerializeField]
    SpriteRenderer tens = null;

    int tensNum = 0;
    int onesNum = 0;

    [SerializeField]
    float enabledTime = 0.0f;
    [SerializeField]
    float rotationTime = 5.0f;

    float counter = 0.0f;

    void Start()
    {
        int value = RoundDataBase.getInstance().PIPILevel[PlayerID];

        tensNum = (Mathf.FloorToInt(value) % 100) / 10;
        onesNum = Mathf.FloorToInt(value) % 10;
    }

    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= enabledTime)
        {
            tens.enabled = true;
            ones.enabled = true;


            if (counter >= enabledTime + rotationTime)
            {
                if (tensNum < 10)
                    tens.sprite = numbars[tensNum];
                if (onesNum < 10)
                    ones.sprite = numbars[onesNum];
            }
            else
            {
                int count = 0;

                if (count % 10 < 10)
                {
                    count = Random.Range(0, 9);
                    tens.sprite = numbars[count % 10];
                    count = Random.Range(0, 9);
                    ones.sprite = numbars[count % 10];
                }
            }
        }
        else
        {
            tens.enabled = false;
            ones.enabled = false;
        }

    }
}