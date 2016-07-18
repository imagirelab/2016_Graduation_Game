//魂のデータを溜めるクラス
//シングルトンにしたのはフィールド全体に落ちている魂は、
//プレイヤー全員にとっても共通のものだから

using UnityEngine;
using System.Collections.Generic;

namespace StaticClass
{
    public class SpiritDataBase
    {
        static SpiritDataBase spiritDataBase = new SpiritDataBase();

        List<GameObject> spirits = new List<GameObject>();

        public void ClearList()
        {
            spirits.Clear();
        }

        public void AddList(GameObject spirit)
        {
            spirits.Add(spirit);
        }

        public void RemoveList(GameObject spirit)
        {
            spirits.Remove(spirit);
        }

        //一番近い魂を返す
        public GameObject GetNearestObject(Vector3 center)
        {
            if (spirits.Count == 0)
                return null;

            GameObject nearestObject = spirits[0];

            foreach(var e in spirits)
            {
                if (Vector3.Distance(center, e.gameObject.transform.position) < Vector3.Distance(center, nearestObject.gameObject.transform.position))
                    nearestObject = e;
            }

            return nearestObject;
        }

        public static SpiritDataBase getInstance()
        {
            return spiritDataBase;
        }
    }
}
