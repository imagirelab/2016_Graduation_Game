using UnityEngine;
using System.Collections;
using NCMB;

public class test : MonoBehaviour
{

	// Use this for initialization
	void Start () {

        NCMBObject testClass = new NCMBObject("TestClass");
        testClass["message"] = "Hello, NCMB!";
        testClass.SaveAsync();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
