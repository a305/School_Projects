using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeoutDisplayController : MonoBehaviour {

    public MainController controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnYesClick()
    {
        controller.SetContinueGame(true);
    }

    public void OnNoClick()
    {
        controller.SetContinueGame(false);
    }
}
