using UnityEngine;
using StaticClass;

public class RoundColor : MonoBehaviour
{
    [SerializeField]
    MeshRenderer[] colorChangeMesh = new MeshRenderer[GameRule.roundCount];

    [SerializeField]
    Material[] mat = new Material[GameRule.roundCount + 1];
    
    void Update()
    {
        int p1win = 0;
        int p2win = 0;

        if (colorChangeMesh != null && GameRule.getInstance().round.Count <= colorChangeMesh.Length)
            for (int i = 0; i < GameRule.getInstance().round.Count; i++)
            {
                switch (GameRule.getInstance().round[i])
                {
                    case Enum.ResultType.Player1Win:
                        colorChangeMesh[p1win].material = mat[0];
                        p1win++;
                        break;
                    case Enum.ResultType.Player2Win:
                        colorChangeMesh[colorChangeMesh.Length - 1 - p2win].material = mat[1];
                        p2win++;
                        break;
                    //case GameRule.ResultType.Draw:
                    //    colorChangeMesh[i].material = mat[2];
                    //    break;
                    default:
                        colorChangeMesh[i].material = mat[3];
                        break;
                }
            }
    }
}
