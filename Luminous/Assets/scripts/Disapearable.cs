using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disapearable : MonoBehaviour, triggerListener {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void triggerAction()
    {
        gameObject.SetActive(false);
    }
}
