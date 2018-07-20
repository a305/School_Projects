using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{

    [SerializeField] Transform beacon;


    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Player") {
			beacon.transform.position = this.transform.position;
			Destroy (gameObject);
		}
    }
}
