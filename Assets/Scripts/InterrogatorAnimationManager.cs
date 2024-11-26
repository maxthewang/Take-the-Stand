using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogatorAnimationManager : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySmackTableAngrily()
    {
        anim.SetTrigger("AngrilySmackTable");
    }

    public void PlayGrabHandcuff()
    {
        anim.SetBool("GrabHandcuff", true);
    }

    public void PlayLeanForward()
    {
        anim.SetBool("LeanForward", true);
    }
    
    public void PlayIntimidate()
    {
        anim.SetBool("Intimidate", true);
    }

    public void PlayCalmDown()
    {
        List<string> bools = new List<string> { "GrabHandcuff", "LeanForward", "Intimidate" };
        foreach (string b in bools)
        {
            anim.SetBool(b, false);
        }
        anim.SetTrigger("Intimidate");
    }
}
