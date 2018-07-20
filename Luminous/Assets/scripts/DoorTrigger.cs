using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
	Animator anim;

	public DoorScript door;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

	}

	// Update is called once per frame
	void Update () {

	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player")
			door.DoorOpens ();
		anim.SetBool("Triggered", true);
		GetComponent<BoxCollider2D>().enabled = false;
		print("triggered");
	}

	void OnTriggerExit2D(Collider2D other){
		//if (other.tag == "Player")
		//	door.DoorCloses();
	}
}
