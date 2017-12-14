// Nathan Pham
// CS451 Autumn 2017
// December 1, 2017
// This script will model the behavior of an asteroid
// The asteroid will go through space, determining whether it hits a spaceship part
// If it does hit a spaceship, the health of the spaceship goes down
// The asteroid will have a time to live before it is destroyed.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour
{
    //public Text health;
    public Transform asteroidBody;
    public float radius;
    TheWorld theworld;
    public float timeToLive = 10f;
    private float currentLife = 0f;
    private Vector3 travellingNormal = Vector3.zero;
    private float speed = 0f;
    private float collideCooldown = 1.5f;
    private float currentCoolDown = 0;
    private bool hitPreviously = false;

    // Use this for initialization
    void Start()
    {

    }

    public void Initialize(TheWorld world)
    {
        theworld = world;
        Debug.Log("Initialized");
    }

    public void SetTravellingDirection(Vector3 dir) { travellingNormal = dir; }
    public void SetSpeed(float s) { speed = s; }

    // Update is called once per frame
    void Update()
    {
        if (hitPreviously)
            currentCoolDown += Time.deltaTime;
        currentLife += Time.deltaTime;
        transform.localPosition += travellingNormal * speed * Time.deltaTime; // travel
        bool isAsteroidCollide = theworld.ProcessCollision(transform, theworld.TheRoot.transform);
        if (isAsteroidCollide)
        {
            if (currentCoolDown >= collideCooldown)
                hitPreviously = false;
            if (!hitPreviously)
                travellingNormal *= -1;
            hitPreviously = true;
        }
        if (currentLife >= timeToLive)
        {
            theworld.asteroids.Remove(this);
            Destroy(this.gameObject);
        }

    }
}