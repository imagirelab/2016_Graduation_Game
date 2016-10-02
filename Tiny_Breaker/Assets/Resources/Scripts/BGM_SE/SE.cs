using UnityEngine;

public class SE : MonoBehaviour
{

    public AudioClip findSE;
    public AudioClip atackSE;

    private AudioSource _audioSource;   //音楽再生用
    private Unit _unit;         //状態確認用

    //Findしたとき一回しか再生しないようにするためのフラグ
    private bool oneCall;

    // Use this for initialization
    void Start ()
    {
        oneCall = false;

        _audioSource = this.GetComponent<AudioSource>();

        _unit = GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(_unit.state == Unit.State.Find)
        {
            if (!oneCall && !_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(findSE);
                oneCall = true;
            }
        }
        else if(_unit.state == Unit.State.Attack)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(atackSE);
                oneCall = false;
            }
        }
        else
        {
            oneCall = false;
        }
	    
	}
}
