using UnityEngine;
using StaticClass;

public class WireFrame : MonoBehaviour {

    [SerializeField]
    Color color = new Color(1.0f, 1.0f, 1.0f);

    MeshRenderer mr;
    
    void Start () {
    }

    void Update()
    {
        MeshFilter mf;
        if (GetComponent<MeshFilter>() != null)
            mf = GetComponent<MeshFilter>();
        else
        {
            this.gameObject.AddComponent<MeshFilter>();
            mf = GetComponent<MeshFilter>();
            Debug.Log(this.ToString() + " MeshFilter null");
        }
        mf.mesh.SetIndices(mf.mesh.GetIndices(0), MeshTopology.LineStrip, 0);

        if (GetComponent<MeshRenderer>() != null)
            mr = GetComponent<MeshRenderer>();
        else
        {
            this.gameObject.AddComponent<MeshRenderer>();
            mr = GetComponent<MeshRenderer>();
            Debug.Log(this.ToString() + " MeshRenderer null");
        }
        mr.enabled = GameRule.getInstance().debugFlag;  //デバッグ表示
        mr.material.color = color;
    }
}
