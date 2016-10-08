using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    [SerializeField]
    Sprite[] numbars = new Sprite[10];

    [SerializeField]
    Image ones = null;
    [SerializeField]
    Image tens = null;
    [SerializeField]
    Image hundreds = null;

    [SerializeField]
    int stateTime = 120;
    float currentTime = 0.0f;

    bool IsCounting = false;

    void Start ()
    {
        if(!IsCounting)
        {
            IsCounting = true;
            currentTime = (float)stateTime;
        }
    }
	
	void Update ()
    {
	    if(IsCounting)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0.0f;
                IsCounting = false;
            }
        }

        int hundredsNum = (Mathf.FloorToInt(currentTime) % 1000) / 100;
        int tensNum = (Mathf.FloorToInt(currentTime) % 100) / 10;
        int onesNum = Mathf.FloorToInt(currentTime) % 10;

        if (hundredsNum < 10)
            hundreds.sprite = numbars[hundredsNum];
        if (tensNum < 10)
            tens.sprite = numbars[tensNum];
        if (onesNum < 10)
            ones.sprite = numbars[onesNum];
    }
}
