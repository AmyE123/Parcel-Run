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
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetFloat("runAnimation", _runAnim);
        GetComponent<Animator>().SetFloat("moveSpeed", _runSpeed);
    }
}
