using UnityEngine;
using StaticClass;

public class DeadCounter : MonoBehaviour
{
    [SerializeField]
    int PlayerID = 0;

    [SerializeField]
    Sprite[] numbars = new Sprite[10];

    [SerializeField]
    SpriteRenderer ones = null;
    [SerializeField]
    SpriteRenderer tens = null;
    [SerializeField]
    SpriteRenderer hundreds = null;
    
    int hundredsNum = 0;
    int tensNum = 0;
    int onesNum = 0;

    [SerializeField]
    float enabledTime = 0.0f;
    [SerializeField]
    float rotationTime = 5.0f;

    float counter = 0.0f;

    void Start()
    {
        int value = RoundDataBase.getInstance().PassesPopCount[PlayerID];

        hundredsNum = (Mathf.FloorToInt(value) % 1000) / 100;
        tensNum = (Mathf.FloorToInt(value) % 100) / 10;
        onesNum = Mathf.FloorToInt(value) % 10;
    }
    
	void Update ()
	{
        counter += Time.deltaTime;

        if (counter >= enabledTime)
        {
            hundreds.enabled = true;
            tens.enabled = true;
            ones.enabled = true;


            if (counter >= enabledTime + rotationTime)
            {
                if (hundredsNum < 10)
                    hundreds.sprite = numbars[hundredsNum];
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
                    hundreds.sprite = numbars[count % 10];
                    count = Random.Range(0, 9);
                    tens.sprite = numbars[count % 10];
                    count = Random.Range(0, 9);
                    ones.sprite = numbars[count % 10];
                }
            }
        }
        else
        {
            hundreds.enabled = false;
            tens.enabled = false;
            ones.enabled = false;
        }

    }
}