using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour {

    [SerializeField]
    AudioSource music;
    [SerializeField]
    GameObject AudioManager;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.GetComponent<SceneMusic>().playMusic(music);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }
    }
}
