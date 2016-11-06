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
            if (dictionary.ContainsKey(key))
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

        //指定したvalue以外の要素だけを取得
        public List<GameObject> GetListToTagExc(string tag)
        {
            List<GameObject> list = new List<GameObject>();

            foreach (GameObject e in dictionary.Keys)
                if (dictionary[e] != tag)
                    list.Add(e);

            return list;
        }

        //リストの中から同じルート番号のものを見つけてリストを再構成
        List<GameObject> GetListToRoot(List<GameObject> chacklist, Enum.Direction_TYPE rootNum)
        {
            List<GameObject> list = new List<GameObject>();

            foreach (GameObject e in chacklist)
                if (e.GetComponent<Unit>())
                    if (e.GetComponent<Unit>().rootNum == rootNum)
                        list.Add(e);

            return list;
        }

        //一番近い悪魔を返す
        public GameObject GetNearestObject(string tag, Vector3 center)
        {
            //指定したタグ以外で一番近いものとする
            List<GameObject> list = GetListToTagExc(tag);

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

        /// <summary>
        /// 指定したルートの中で一番近い兵士を返す
        /// </summary>
        /// <param name="tag">検索しないタグ名</param>
        /// <param name="center">中心点</param>
        /// <param name="rootNum">ルート番号</param>
        /// <returns>一番近い兵士</returns>
        public GameObject GetNearestObject(string tag, Vector3 center, Enum.Direction_TYPE rootNum)
        {
            //指定したタグ以外で一番近いものとする
            List<GameObject> list = GetListToTagExc(tag);

            //ルート番号か同じものだけを抽出する
            list = GetListToRoot(list, rootNum);

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
