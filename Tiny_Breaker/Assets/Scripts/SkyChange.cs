using UnityEngine;
using StaticClass;

public class SkyChange : MonoBehaviour
{
    [SerializeField]
    GameObject[] skyObj = new GameObject[GameRule.roundCount];

    [SerializeField]
    Light[] lights = new Light[5];
    [SerializeField]
    Color[] lightColor = new Color[GameRule.roundCount];
    [SerializeField]
    float[] lightIntensity = new float[GameRule.roundCount];

    void Start ()
	{
        int roundNum = GameRule.getInstance().round.Count;

        //空の切り替え
        foreach (GameObject e in skyObj)
            e.SetActive(false);

        skyObj[roundNum].SetActive(true);

        //ライトの切り替え
        foreach(Light e in lights)
        {
            e.color = lightColor[roundNum];
            e.intensity = lightIntensity[roundNum];
        }
	}
}