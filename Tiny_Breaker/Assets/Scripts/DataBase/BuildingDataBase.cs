//建物のデータを溜めるクラス

using UnityEngine;
using System.Collections.Generic;

namespace StaticClass
{
    public class BuildingDataBase
    {
        static BuildingDataBase dataBase = new BuildingDataBase();

        public static BuildingDataBase getInstance()
        {
            return dataBase;
        }

        List<GameObject> list = new List<GameObject>();

        public void ClearList()
        {
            list.Clear();
        }

        public void AddList(GameObject item)
        {
            if(!ChackKey(item))
                list.Add(item);
        }

        public void RemoveList(GameObject item)
        {
            if(list.Contains(item))
                list.Remove(item);
        }

        //渡されたオブジェクトがリストにあるかどうかチェックする
        public bool ChackKey(GameObject item)
        {
            return list.Contains(item);
        }

        //一番近い魂を返す
        public GameObject GetNearestObject(Vector3 center)
        {
            if (list.Count == 0)
                return null;

            GameObject nearestObject = list[0];

            foreach (var e in list)
            {
                if (Vector3.Distance(center, e.gameObject.transform.position) < Vector3.Distance(center, nearestObject.gameObject.transform.position))
                    nearestObject = e;
            }

            return nearestObject;
        }
    }
}
