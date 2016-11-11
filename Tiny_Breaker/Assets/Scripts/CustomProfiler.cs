using UnityEngine;

public class CustomProfiler : MonoBehaviour
{
	void Start ()
    {
        Profiler.logFile = "mylog.log";
        Profiler.enabled = true;
        Profiler.maxNumberOfSamplesPerFrame = -1;
    }
}