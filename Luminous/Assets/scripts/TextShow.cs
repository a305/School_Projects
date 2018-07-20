using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShow : MonoBehaviour, triggerListener {

    [SerializeField]
    Text hint;
	// Use this for initialization
	void Start () {
        hint.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void triggerAction()
    {
        hint.enabled = !hint.enabled;
    }

    
}
