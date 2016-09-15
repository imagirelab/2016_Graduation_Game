﻿//目標物を設定するコルーチン
//索敵レーダーみたいだといいかも

using UnityEngine;
using System.Collections;

public class UnitSeach : MonoBehaviour
{
    Coroutine cor;
    
    IEnumerator Search()
    {
        Unit unit = gameObject.GetComponent<Unit>();
        
        while (true)
        {
            //プレイヤーのTarget
            unit.SetNearTargetObject();
            
            yield return new WaitForSeconds(0.0f);
        }
    }

    void OnEnable()
    {
        cor = StartCoroutine(Search());
    }

    void OnDisable()
    {
        StopCoroutine(cor);
    }
}