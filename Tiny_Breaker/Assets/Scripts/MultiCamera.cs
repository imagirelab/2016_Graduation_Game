using UnityEngine;

public class MultiCamera : MonoBehaviour
{
    public int maxDisplayCount = 1;

    void Start ()
	{
        for(int i = 0; i < maxDisplayCount && i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
	}
}
