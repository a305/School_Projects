using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Transform target;
    public float velocity = 10;

    private Rigidbody2D body;
    private int direction;
    private FlashLightControl flashLight;
    private bool inBeam;

	// Use this for initialization
	void Start () {
        inBeam = false;
        body = GetComponent<Rigidbody2D>();
        direction = -1;
        flashLight = GameObject.FindWithTag("Light").transform.parent.GetComponent<FlashLightControl>();

        if (target == null)
            target = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 difference = transform.position - target.position;

        if (inBeam == true && flashLight.GetOnStatus() == true)
            direction = 1;
        else
            direction = -1;

        if (difference.magnitude <= 15)
            transform.position += direction * difference.normalized * Time.deltaTime * velocity;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Light")
            inBeam = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Light")
            inBeam = false;
    }
}
