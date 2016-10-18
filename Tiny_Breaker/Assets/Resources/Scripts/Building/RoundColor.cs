using UnityEngine;
using StaticClass;

public class RoundColor : MonoBehaviour
{
    [SerializeField]
    MeshRenderer[] colorChangeMesh = new MeshRenderer[GameRule.roundCount];

    [SerializeField]
    Material[] mat = new Material[GameRule.roundCount + 1];

    void Start()
    {

    }

    void Update()
    {
        if (colorChangeMesh != null && GameRule.getInstance().round.Count <= colorChangeMesh.Length)
            for (int i = 0; i < GameRule.getInstance().round.Count; i++)
            {
                switch (GameRule.getInstance().round[i])
                {
                    case GameRule.ResultType.Player1Win:
                        colorChangeMesh[i].material = mat[0];
                        break;
                    case GameRule.ResultType.Player2Win:
                        colorChangeMesh[i].material = mat[1];
                        break;
                    case GameRule.ResultType.Draw:
                        colorChangeMesh[i].material = mat[2];
                        break;
                    default:
                        colorChangeMesh[i].material = mat[3];
                        break;
                }
            }
    }
}
