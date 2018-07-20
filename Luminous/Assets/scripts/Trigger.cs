using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject listener;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown("Action"))
            {
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                listener.GetComponent<triggerListener>().triggerAction();
            }
        }
    }
}
