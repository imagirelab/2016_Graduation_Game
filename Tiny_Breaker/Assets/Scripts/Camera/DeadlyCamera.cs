using UnityEngine;
using System.Collections;

public class DeadlyCamera : MonoBehaviour
{
    public GameObject targetObj;

    public GameObject[] particle;
    private GameObject[] _particle = { null, null, null};

    public float moveYSpeed = 1;
    public float moveRotateSpeed = 1;
    public float rotateTime = 1;

    public AudioClip[] SE;

    AudioSource _auido;

    float timer;
    float nowSpeed;

    int frame = 0;

    bool StopFlag = false;

    // Use this for initialization
    void Start ()
    {
        _auido = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(timer < rotateTime && !StopFlag)
        {
            timer += Time.deltaTime;
            transform.position -= new Vector3(0, moveYSpeed, 0);
            nowSpeed = Mathf.Sin(timer * Mathf.Deg2Rad) * moveRotateSpeed;
        }
        else
        {
            StopFlag = true;

            if(timer > 0)
            {
                timer -= Time.deltaTime * rotateTime;
            }
            else
            {
                timer = 0;
            }            
            nowSpeed = Mathf.Sin(timer * Mathf.Deg2Rad) * moveRotateSpeed;
        }
        
        Vector3 axis = transform.TransformDirection(Vector3.up);
        transform.RotateAround(targetObj.transform.position + new Vector3(0, 10, 0), axis, nowSpeed);    

        transform.LookAt(targetObj.transform.position + new Vector3(0, 10, 0));

        if(frame > 66 && _particle[0] == null)
        {
            _particle[0] = Instantiate(particle[0]);
            _particle[0].transform.position = targetObj.transform.position;
            _auido.clip = SE[0];
            _auido.Play();
        }
        if (frame > 138 && _particle[1] == null)
        {
            _particle[1] = Instantiate(particle[1]);
            _particle[1].transform.position = targetObj.transform.position;
            _auido.clip = SE[1];
            _auido.Play();
        }
        if (frame > 234 && _particle[2] == null)
        {
            _particle[2] = Instantiate(particle[2]);
            _particle[2].transform.position = targetObj.transform.position + new Vector3(0, 17, 0);
            _auido.clip = SE[2];
            _auido.Play();
        }

        frame++;
    }
}
