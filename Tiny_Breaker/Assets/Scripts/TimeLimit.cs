using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    [SerializeField]
    int stateTime = 120;

    //普通の数字画像
    [SerializeField]
    Sprite[] numbars = new Sprite[10];
    //ラスト～秒のときの数字画像
    [SerializeField]
    Sprite[] lastNumbars = new Sprite[10];
    //番号スプライト配列の入れ物
    Sprite[] spriteNumbars = new Sprite[10];

    [SerializeField]
    Image ones = null;
    [SerializeField]
    Image tens = null;
    [SerializeField]
    Image hundreds = null;

    float currentTime = 0.0f;

    bool IsCounting = false;

    bool end = false;
    public bool End { get { return end; } }

    //最後何秒を知らせるオブジェクト
    public GameObject thirtyObj;
    bool thirtyFlag = false;
    public GameObject sixtyObj;
    bool sixtyFlag = false;

    public GameObject[] lastNumObj = new GameObject[10];
    bool[] lastNumFlag = new bool[10];

    public AudioClip[] lastVoices;

    AudioSource _audio;

    void Start ()
    {
        end = false;
        thirtyFlag = false;
        sixtyFlag = false;

        for (int i = 0; i < lastNumFlag.Length; i++)
            lastNumFlag[i] = false;

        if (!IsCounting)
        {
            IsCounting = true;
            currentTime = (float)stateTime;
        }

        //基本のナンバー配列を設定
        spriteNumbars = numbars;

        _audio = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        if (IsCounting)
        {
            currentTime -= Time.deltaTime;

            //０秒をきったときの処理
            if (currentTime <= 1)
            {
                end = true;

                currentTime = 0.0f;
                IsCounting = false;
            }

            //１０秒をきったときの処理
            if (currentTime < 11)
            {
                //基本のナンバー配列を設定
                spriteNumbars = lastNumbars;

                if (!lastNumFlag[0])
                {
                    lastNumFlag[0] = true;

                    GameObject instace = (GameObject)Instantiate(lastNumObj[0],
                                                             lastNumObj[0].transform.position,
                                                             Quaternion.identity);
                    StartCoroutine(instace.GetComponent<ScaleMove>().ScaleUpDown());

                    _audio.clip = lastVoices[0];
                    _audio.Play();
                }
            }
            
            //３０秒をきったときの処理
            if (currentTime <= 31 && !thirtyFlag)
            {
                thirtyFlag = true;

                GameObject instace = (GameObject)Instantiate(thirtyObj,
                                                             thirtyObj.transform.position,
                                                             Quaternion.identity);
                StartCoroutine(instace.GetComponent<ScaleMove>().ScaleUpDown());
            }

            //６０秒をきったときの処理
            if (currentTime <= 61 && !sixtyFlag)
            {
                sixtyFlag = true;

                GameObject instace = (GameObject)Instantiate(sixtyObj,
                                                             sixtyObj.transform.position,
                                                             Quaternion.identity);
                StartCoroutine(instace.GetComponent<ScaleMove>().ScaleUpDown());
            }
        }

        int hundredsNum = (Mathf.FloorToInt(currentTime) % 1000) / 100;
        int tensNum = (Mathf.FloorToInt(currentTime) % 100) / 10;
        int onesNum = Mathf.FloorToInt(currentTime) % 10;

        if (hundredsNum < 10)
            hundreds.sprite = spriteNumbars[hundredsNum];
        if (tensNum < 10)
            tens.sprite = spriteNumbars[tensNum];
        if (onesNum < 10)
            ones.sprite = spriteNumbars[onesNum];
        
        //９秒をきったときの処理
        if (!lastNumFlag[onesNum] && currentTime < 10)
        {
            lastNumFlag[onesNum] = true;

            GameObject instace = (GameObject)Instantiate(lastNumObj[onesNum],
                                                         lastNumObj[onesNum].transform.position,
                                                         Quaternion.identity);
            StartCoroutine(instace.GetComponent<ScaleMove>().ScaleUpDown());

            _audio.clip = lastVoices[onesNum];
            _audio.Play();
        }
    }
}
