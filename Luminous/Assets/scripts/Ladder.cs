using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, triggerListener {

	void Start(){
		setAllCollidersStatus (false);
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player") {
			other.GetComponent<Rigidbody2D> ().gravityScale = 0;
			if (Input.GetKey (KeyCode.W)) {
				other.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 9);
		
			}else if(Input.GetKey (KeyCode.S)) {
				other.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -8);
			}
			else
				other.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0); //stops them from climbing
			}
		//do nothing
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			other.GetComponent<Rigidbody2D> ().gravityScale = 3;
		}
	}

	public void triggerAction()
	{
		GetComponent<SpriteRenderer> ().enabled = true;
		setAllCollidersStatus (true);
	}

	public void setAllCollidersStatus(bool status){
		foreach(Collider2D c in GetComponents<Collider2D>()){
			c.enabled = status;
		}	
	}
}
