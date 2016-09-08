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

        Dictionary<GameObject, string> dictionary = new Dictionary<GameObject, string>();

        public void ClearList()
        {
            dictionary.Clear();
        }

        public void AddList(GameObject key, string value)
        {
            dictionary.Add(key, value);
        }

        public void RemoveList(GameObject key)
        {
            dictionary.Remove(key);
        }

        //辞書にある数の取得
        public int GetCount()
        {
            return dictionary.Count;
        }

        //指定したvalueの要素だけを取得
        public List<GameObject> GetListToTag(string tag)
        {
            List<GameObject> list = new List<GameObject>();

            foreach (GameObject e in dictionary.Keys)
                if (dictionary[e] == tag)
                    list.Add(e);

            return list;
        }

        //一番近い悪魔を返す
        public GameObject GetNearestObject(string tag, Vector3 center)
        {
            //指定したタグの中で一番近いものとする
            List<GameObject> list = GetListToTag(tag);

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
        public Vector3 GetCenterPosition(string tag)
        {
            //指定したタグの中で一番近いものとする
            List<GameObject> list = GetListToTag(tag);

            if (list.Count == 0)
                return Vector3.zero;

            Vector3 center = Vector3.zero;

            center = (GetMaxPosition(tag) + GetMinPosition(tag)) * 0.5f;

            return center;
        }

        //出ている悪魔達の最大座標を返す
        public Vector3 GetMaxPosition(string tag)
        {
            //指定したタグの中で一番近いものとする
            List<GameObject> list = GetListToTag(tag);

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
        public Vector3 GetMinPosition(string tag)
        {
            //指定したタグの中で一番近いものとする
            List<GameObject> list = GetListToTag(tag);

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
