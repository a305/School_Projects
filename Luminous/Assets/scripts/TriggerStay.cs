using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStay : MonoBehaviour {

    [SerializeField]
    GameObject listener;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            listener.GetComponent<triggerListener>().triggerAction();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            listener.GetComponent<triggerListener>().triggerAction();
        }
    }
}
