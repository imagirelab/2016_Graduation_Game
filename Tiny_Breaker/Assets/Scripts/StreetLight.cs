using UnityEngine;
using StaticClass;

public class StreetLight : MonoBehaviour
{
    [SerializeField]
    bool[] roundLighting = new bool[GameRule.roundCount];

	void Start ()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        if (roundLighting[GameRule.getInstance().round.Count])
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }
	}
}