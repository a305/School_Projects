using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseChar : MonoBehaviour {

    [SerializeField]PlayerController player;
    
	// Use this for initialization
	void Start () {
		gameObject.SetActive(true);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
           
        //if player touches you destroy your self
        if (collision.tag == "Player")
        {
            player.invControl = true;
            player.startTime = Time.time;
			gameObject.SetActive(false);
        }
    }
}
