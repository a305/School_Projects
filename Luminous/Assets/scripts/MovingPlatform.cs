using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, triggerListener {


	public GameObject platform;
	public float moveSpeed;

	private Transform curPoint;

	public Transform[] points;

	public int pointSelection;

	void Start(){
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		curPoint = points [pointSelection];
	}

	void Update(){
		transform.position = Vector3.MoveTowards(transform.position, curPoint.position, Time.deltaTime * moveSpeed);
	
		if (transform.position == curPoint.position) {
			pointSelection++;

			if (pointSelection == points.Length) {
				pointSelection = 0;
			}

			curPoint = points [pointSelection];
		}
	}


	private void OnCollisionEnter2D(Collision2D col){
		if (col.transform.tag == "Player") {
			col.collider.transform.SetParent (transform);
		}
	}

	private void OnCollisionExit2D(Collision2D col){
		if (col.transform.tag == "Player") {
			col.collider.transform.SetParent (null);
		}
	}

	public void triggerAction()
	{
		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<BoxCollider2D>().enabled = true;
	}
}
