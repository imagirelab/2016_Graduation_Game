using UnityEngine;

public class ChangeModel : MonoBehaviour
{
    [SerializeField]
    Avatar[] avatars = new Avatar[2];
    [SerializeField]
    GameObject[] models = new GameObject[2];

    [SerializeField]
    Unit unit = null;

	void Start ()
    {
        if (unit == null)
            return;

        Animator anim = GetComponent<Animator>();
        AnimatorControllerParameter[] parameters = anim.parameters;

        foreach (GameObject e in models)
            e.SetActive(false);
        
        if (unit.level >= 10)
        {
            anim.avatar = avatars[1];
            models[1].SetActive(true);

            foreach (AnimatorControllerParameter param in parameters)
                if (param.name == "IsLevelUP")
                    anim.SetBool("IsLevelUP", true);
        }
        else
        {
            anim.avatar = avatars[0];
            models[0].SetActive(true);

            foreach (AnimatorControllerParameter param in parameters)
                if (param.name == "IsLevelUP")
                    anim.SetBool("IsLevelUP", false);
        }
    }
}