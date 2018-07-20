using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour {

    private bool isPaused;

    [SerializeField]
    Canvas pauseMenu;
	// Use this for initialization
	void Start () {
        start();
        pauseMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
                start();
            else
                pause();
        }
	}

    public void pause()
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseMenu.enabled = true;
    }

    public void start()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.enabled = false;
    }
}
