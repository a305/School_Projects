// Nathan Pham
// CS451 Autumn 2017
// December 5, 2017
// This script is to model the behavior of the bullet. If the bullet hits a leaf node
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timeToLive = 3f;
    private float currentLife = 0f;
    private Vector3 travellingNormal = Vector3.zero;
    private float speed = 0f;
    private TheWorld world;

    public void SetTravellingDirection(Vector3 dir) { travellingNormal = dir; }
    public void SetSpeed(float s) { speed = s; }
    public void SetWorld(ref TheWorld w) { world = w; }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (world.TheRoot.transform == null)
            return;
        currentLife += Time.deltaTime;
        transform.localPosition += travellingNormal * speed * Time.deltaTime; // travel
        bool isCollide = world.ProcessCollision(transform, world.TheRoot.transform);
        if (isCollide)
        {
            world.TheRoot.transform.localPosition += speed * transform.right * Time.deltaTime;
        }
        if (currentLife >= timeToLive)
            Destroy(gameObject);
    }
}
