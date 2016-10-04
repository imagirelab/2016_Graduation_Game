using UnityEngine;
using UnityEngine.UI;

public class DefenseBaseIcon : MonoBehaviour
{
    [SerializeField]
    DefenseBase player;

    [SerializeField]
    Sprite iconFine = new Sprite();
    [SerializeField]
    Sprite iconPinch = new Sprite();

    [SerializeField]
    float pinch = 0.25f;

    Image image;

    void Start()
    {
        if (player == null)
            player = new DefenseBase();

        if (GetComponent<Image>())
            image = GetComponent<Image>();
    }

    void Update()
    {
        float HPRate = (float)player.HPpro / (float)player.GetHP;

        if(HPRate <= pinch)
            image.sprite = iconPinch;
        else
            image.sprite = iconFine;
    }
}
