//兵士のデータを溜めるクラス
//シングルトンにしたのはフィールド全体の兵士は、
//プレイヤー全員にとっても共通のものだから

using UnityEngine;
using System.Collections.Generic;

namespace StaticClass
{
    public class SolgierDataBase
    {
        static SolgierDataBase dataBase = new SolgierDataBase();
        
        public static SolgierDataBase getInstance()
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
    }
}
