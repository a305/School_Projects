using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void DoorOpens()
	{
        anim.SetBool("Opening", true);
        GetComponent<BoxCollider2D>().enabled = false;
        print("open");
	}

	public void DoorCloses()
	{
        anim.SetBool("Opening", false);
        GetComponent<BoxCollider2D>().enabled = true;

        print("close");
	}
}
