using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenPerson : MonoBehaviour
{
    [SerializeField]
    private float _runSpeed;

    [SerializeField]
    private float _runAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();

        anim.SetFloat("runAnimation", _runAnim);
        anim.SetFloat("moveSpeed", _runSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
