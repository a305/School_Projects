using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour {

	public float time;
	bool moving;
	float start;

	// Use this for initialization
	void Start () {	
		moving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving == false && gameObject.GetComponent<Rigidbody2D>().velocity.y < -10) {
			moving = true;
			start = Time.time;
		}

		if (moving == true) {
            if (Time.time - start > time)
                gameObject.SetActive(false);
		}	
	}
}
