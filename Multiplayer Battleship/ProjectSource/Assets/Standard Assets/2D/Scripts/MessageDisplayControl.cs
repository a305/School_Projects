using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplayControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnOkClick()
    {
       transform.parent.gameObject.SetActive(false);
    }
}
