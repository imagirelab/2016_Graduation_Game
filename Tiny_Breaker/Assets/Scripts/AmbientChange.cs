using UnityEngine;
using StaticClass;

public class AmbientChange : MonoBehaviour
{
    [SerializeField]
    Color[] lightColor = new Color[GameRule.roundCount];
    [SerializeField]
    float[] lightIntensity = new float[GameRule.roundCount];

    void Start ()
    {
        int roundNum = GameRule.getInstance().round.Count;

        RenderSettings.ambientLight = lightColor[roundNum];
        RenderSettings.ambientIntensity = lightIntensity[roundNum];
    }
	
	void Update ()
	{
		
	}
}