using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour {

	bool onTop;
	GameObject bouncer;
	public Vector2 velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay2D(Collision2D other){
		if (onTop) {
			bouncer = other.gameObject;
			jump ();
		}
		
	}



	void OnTriggerEnter2D(){
		onTop = true;
	}
	void OnTriggerExit2D(){
		onTop = false;
	}
	void OnTriggerStay2D(){
		onTop = true;
	}

	void jump(){
		bouncer.GetComponent<Rigidbody2D>().velocity = velocity;
	}
}
