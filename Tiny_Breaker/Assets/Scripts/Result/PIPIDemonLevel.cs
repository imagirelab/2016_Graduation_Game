﻿using UnityEngine;
using UnityEngine.UI;
using StaticClass;

public class PIPIDemonLevel : MonoBehaviour
{
    [SerializeField]
    int PlayerID = 0;

    [SerializeField]
    Sprite[] numbars = new Sprite[10];

    [SerializeField]
    Image ones = null;
    [SerializeField]
    Image tens = null;

    void Start()
    {

    }

    void Update()
    {
        int value = RoundDataBase.getInstance().PIPILevel[PlayerID];

        int tensNum = (Mathf.FloorToInt(value) % 100) / 10;
        int onesNum = Mathf.FloorToInt(value) % 10;

        if (tensNum < 10)
            tens.sprite = numbars[tensNum];
        if (onesNum < 10)
            ones.sprite = numbars[onesNum];
    }
}