//兵士のデータを溜めるクラス
//シングルトンにしたのはフィールド全体の兵士は、
//プレイヤー全員にとっても共通のものだから

using UnityEngine;
using System.Collections.Generic;

namespace StaticClass
{
    public class SolgierDataBase
    {
        static SolgierDataBase solgierDataBase = new SolgierDataBase();

        List<GameObject> solgiers = new List<GameObject>();

        public void ClearList()
        {
            solgiers.Clear();
        }

        public void AddList(GameObject solgier)
        {
            solgiers.Add(solgier);
        }

        public void RemoveList(GameObject solgier)
        {
            solgiers.Remove(solgier);
        }

        //一番近い魂を返す
        public GameObject GetNearestObject(Vector3 center)
        {
            if (solgiers.Count == 0)
                return null;

            GameObject nearestObject = solgiers[0];

            foreach (var e in solgiers)
            {
                if (Vector3.Distance(center, e.gameObject.transform.position) < Vector3.Distance(center, nearestObject.gameObject.transform.position))
                    nearestObject = e;
            }

            return nearestObject;
        }

        public static SolgierDataBase getInstance()
        {
            return solgierDataBase;
        }
    }
}
