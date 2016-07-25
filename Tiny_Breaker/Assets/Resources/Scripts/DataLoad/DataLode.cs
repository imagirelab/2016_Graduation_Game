//参考 http://qiita.com/kyubuns/items/bcbe92a18dffea684fbc

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Loader
{
    public class DataLode<T> where T : MasterBase, new()
    {
        protected List<T> masters;
        public List<T> All { get { return masters; } }

        //public string paramurl = "https://yoo3006.github.io/ParamData.csv";
        //public string growurl = "https://yoo3006.github.io/GrowData.csv";
        //public string costurl = "https://yoo3006.github.io/CostData.csv";

        //[SerializeField]
        //GameObject prePOPO;
        //[SerializeField]
        //GameObject prePUPU;
        //[SerializeField]
        //GameObject prePIPI;
        //[SerializeField]
        //GameObject preShield;
        //[SerializeField]
        //GameObject preAx;
        //[SerializeField]
        //GameObject preGun;

        //[SerializeField]
        //GameObject playerCost;
        
        public void Load(string text)
        {
            //WWW www = new WWW(filePath);
            //WWW paramwww = new WWW(paramurl);
            //WWW growwww = new WWW(growurl);
            //WWW costwww = new WWW(costurl);
            //yield return www;
            //yield return paramwww;
            //yield return growwww;
            //yield return costwww;

            //string text = www.text;
            //string paramtext = paramwww.text;
            //string growtext = growwww.text;
            //string costtext = costwww.text;

            text = text.Trim().Replace("\r", "") + "\n";
            List<string> lines = text.Split('\n').ToList();

            // header
            string[] headerElements = lines[0].Split(',');
            lines.RemoveAt(0); // header部分を削除
            
            // body
            masters = new List<T>();
            foreach (var line in lines) ParseLine(line, headerElements);
        }

        private void ParseLine(string line, string[] headerElements)
        {
            var elements = line.Split(',');
            if (elements.Length == 1) return;
            if (elements.Length != headerElements.Length)
            {
                Debug.LogWarning(string.Format("can't load: {0}", line));
                return;
            }

            var param = new Dictionary<string, string>();
            for (int i = 0; i < elements.Length; i++) param.Add(headerElements[i], elements[i]);
            var master = new T();
            master.Load(param);
            masters.Add(master);
        }
    }

    public class MasterBase
    {
        public void Load(Dictionary<string, string> param)
        {
            foreach (string key in param.Keys)
                SetField(key, param[key]);
        }

        private void SetField(string key, string value)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (propertyInfo.PropertyType == typeof(string)) propertyInfo.SetValue(this, value, null);
            else if (propertyInfo.PropertyType == typeof(int)) propertyInfo.SetValue(this, int.Parse(value), null);
            else if (propertyInfo.PropertyType == typeof(float)) propertyInfo.SetValue(this, float.Parse(value), null);
            // 他の型にも対応させたいときには適当にここに。enumとかもどうにかなりそう。
        }
    }
}