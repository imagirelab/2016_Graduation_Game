using UnityEngine;

public class SE : MonoBehaviour
{

    public AudioClip findSE;
    public AudioClip atackSE;
    
    private Unit _unit;         //状態確認用
    
    void Start ()
    {
        _unit = GetComponent<Unit>();
	}
	
	void Update ()
    {
        if(_unit.state == Enum.State.Find && !SoundManager.findSEFlag)
        {
            SoundManager.findSEFlag = true;
        }
        else if(_unit.state == Enum.State.Attack && !SoundManager.atackSEFlag)
        {
            SoundManager.atackSEFlag = true;
        }
	}
}
