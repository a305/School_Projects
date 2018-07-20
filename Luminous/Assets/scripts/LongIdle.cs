using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongIdle : MonoBehaviour {

 
    Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        /*
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("playerIdle") && stateInfo.normalizedTime > 50)
        {
           // animator.SetBool("longIdle", true); 
        }
        else if (stateInfo.IsName("scratchAnimation"))
        {
            print(stateInfo.normalizedTime);
            animator.SetBool("longIdle", false);
        }
        */
	}
}
