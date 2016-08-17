using UnityEngine;
using StaticClass;

public class SpawnPoint : MonoBehaviour {

    [SerializeField]
    float speed = 1.0f;
    Vector3 defaultPosition = Vector3.zero;
    
	void Start ()
    {
        defaultPosition = transform.position;
    }
	
	void Update ()
    {
        Vector3 targetPosition = DemonDataBase.getInstance().GetCenterPosition();

        if (DemonDataBase.getInstance().GetListCount() == 0)
            targetPosition = defaultPosition;
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.destination = targetPosition;
    }
}
