using UnityEngine;
using StaticClass;

public class RoundCountUI : MonoBehaviour
{
    [SerializeField]
    Enum.ResultType type = Enum.ResultType.None;
    [SerializeField]
    GameObject image;
    [SerializeField]
    float distance = -55.0f;

    void Start ()
    {
        if (image == null)
            image = new GameObject();

        int wincount = 0;
        foreach (var e in GameRule.getInstance().round)
            if (e == type)
            {
                GameObject instace = (GameObject)Instantiate(image, image.transform.position, Quaternion.identity);
                instace.transform.SetParent(this.transform, false);
                instace.GetComponent<RectTransform>().position += new Vector3(distance * wincount, 0.0f, 0.0f);
                wincount++;
            }


    }
}