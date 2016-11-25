using UnityEngine;
using StaticClass;

public class DirectionChange : MonoBehaviour
{
    [SerializeField]
    Vector3[] rot = new Vector3[GameRule.roundCount];

    [SerializeField]
    Color[] lightColor = new Color[GameRule.roundCount];
    [SerializeField]
    float[] lightIntensity = new float[GameRule.roundCount];

    void Start()
    {
        int roundNum = GameRule.getInstance().round.Count;

        Light light = GetComponent<Light>();
        light.color = lightColor[roundNum];
        light.intensity = lightIntensity[roundNum];

        Vector3 rotation = rot[roundNum];
        this.transform.localRotation = Quaternion.Euler(rotation);
    }
}