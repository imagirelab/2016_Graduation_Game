using UnityEngine;
using System.Collections;

public class DeadlyCamera : MonoBehaviour
{
    public GameObject targetObj;

    public float moveYSpeed = 1;
    public float moveRotateSpeed = 1;
    public float rotateTime = 1;

    float timer;
    float nowSpeed;

    bool StopFlag = false;

    // Use this for initialization
    void Start ()
    {
	    
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
    }
}
