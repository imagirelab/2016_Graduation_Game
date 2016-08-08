using UnityEngine;
using StaticClass;

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    GameObject unit;

    GameObject _MainCamera;

    [SerializeField]
    GameObject guageSprite;
    
    void Start()
    {
        if(unit == null)
            unit = new GameObject(this.ToString() + " unit");

        _MainCamera = GameObject.Find("Main Camera");

        if (guageSprite == null)
            guageSprite = new GameObject(this.ToString() + " guageSprite");
    }

    void Update()
    {
        transform.LookAt(_MainCamera.transform);
        transform.forward = _MainCamera.transform.forward;

        //デバッグによってオンオフ切り替え
        if (transform.GetComponent<SpriteRenderer>() != null)
            transform.GetComponent<SpriteRenderer>().enabled = !GameRule.getInstance().debugFlag;
        foreach (Transform child in transform)
            if (transform.GetComponent<SpriteRenderer>() != null)
                child.GetComponent<SpriteRenderer>().enabled = !GameRule.getInstance().debugFlag;

        if (!GameRule.getInstance().debugFlag)
        {
            float HPRate = (float)unit.GetComponent<Unit>().status.CurrentHP / (float)unit.GetComponent<Unit>().status.MaxHP;
            guageSprite.transform.localScale = new Vector3( HPRate, 1.0f, 1.0f);
        }
    }
}
