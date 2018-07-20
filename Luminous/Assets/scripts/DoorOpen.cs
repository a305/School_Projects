using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour, triggerListener {

    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void triggerAction()
    {

        anim.SetBool("Opening", true);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
