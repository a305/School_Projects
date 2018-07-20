using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetable : MonoBehaviour {

    private Vector3 location;
	void Start () {
        gameObject.tag = "reset";
        location = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = location;
    }
}
