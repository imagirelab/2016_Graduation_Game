//悪魔のデータを溜めるクラス

using UnityEngine;
using System.Collections.Generic;

namespace StaticClass
{
    public class DemonDataBase
    {
        static DemonDataBase dataBase = new DemonDataBase();

        public static DemonDataBase getInstance()
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
            list.Add(item);
        }

        public void RemoveList(GameObject item)
        {
            list.Remove(item);
        }

        //リストにある数の取得
        public int GetListCount()
        {
            return list.Count;
        }

        //一番近い悪魔を返す
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

        //出ている悪魔達の中心点を返す
        public Vector3 GetCenterPosition()
        {
            if (list.Count == 0)
                return Vector3.zero;

            Vector3 center = Vector3.zero;

            center = (GetMaxPosition() + GetMinPosition()) * 0.5f;

            return center;
        }

        //出ている悪魔達の最大座標を返す
        public Vector3 GetMaxPosition()
        {
            if (list.Count == 0)
                return Vector3.zero;

            Vector3 max = list[0].transform.position;

            foreach (var e in list)
            {
                if (max.x < e.transform.position.x)
                    max.x = e.transform.position.x;
                if (max.y < e.transform.position.y)
                    max.y = e.transform.position.y;
                if (max.z < e.transform.position.z)
                    max.z = e.transform.position.z;
            }

            return max;
        }

        //出ている悪魔達の最小座標を返す
        public Vector3 GetMinPosition()
        {
            if (list.Count == 0)
                return Vector3.zero;

            Vector3 min = list[0].transform.position;

            foreach (var e in list)
            {
                if (min.x > e.transform.position.x)
                    min.x = e.transform.position.x;
                if (min.y > e.transform.position.y)
                    min.y = e.transform.position.y;
                if (min.z > e.transform.position.z)
                    min.z = e.transform.position.z;
            }

            return min;
        }
    }
}
