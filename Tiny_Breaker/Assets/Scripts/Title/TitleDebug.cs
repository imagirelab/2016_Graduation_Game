using UnityEngine;
using UnityEngine.UI;

public class TitleDebug : MonoBehaviour
{
    public Text text;

    bool flag = false;
    
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            flag = !flag;

            if(flag)
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
            else
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
        }
	}
}