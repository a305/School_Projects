using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour {

    [SerializeField]
    AudioSource source;


    float songTime;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {

        songTime = source.time;
	}

    public void playMusic(AudioSource source)
    {

        this.source.Stop();
        this.source = source;
        source.time = songTime;
        this.source.Play();
    }

}
