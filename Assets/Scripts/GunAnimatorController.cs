using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimatorController : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTimerEnd()
    {
        animator.SetTrigger("Raise Gun");
    }
}
