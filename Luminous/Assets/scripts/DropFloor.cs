using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFloor : MonoBehaviour{

	// Use this for initialization
	void Start () {
		gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			gameObject.SetActive(false);
		}
	}
}
