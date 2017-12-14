// Nathan Pham
// CS451 Autumn 2017
// December 5th, 2017
// This script models the behavior of the stationary sentry
// Where the sentry looks at the spaceship and launches bullets at the spaceship
// With a random spread
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationarySentry : MonoBehaviour
{
    public TheWorld world;
    public Transform barrel;
    private List<Bullet> bullets = new List<Bullet>();
    private float bulletGenInterval = 2f;
    private float currentGenTime = 0;
    private float bulSpeed = 20f;
	// Use this for initialization
	void Start ()
    {
	}

    private void FixedUpdate()
    {
        if(world.TheRoot != null)
        transform.LookAt(world.TheRoot.transform);
    }

    // Update is called once per frame
    void Update ()
    {
        if (world.IsPlayMode())
        {
            currentGenTime += Time.deltaTime;
            if (currentGenTime >= bulletGenInterval)
            {
                GameObject bullet = Instantiate(Resources.Load("Prefabs\\Bullet")) as GameObject;
                bullet.GetComponent<Bullet>().SetWorld(ref world);
                bullet.transform.localPosition = transform.localPosition + 2 * barrel.transform.up;
                bullet.GetComponent<Bullet>().SetSpeed(bulSpeed);
                bullet.GetComponent<Bullet>().SetTravellingDirection(barrel.transform.up);
                bullets.Add(bullet.GetComponent<Bullet>());
                currentGenTime = 0;
            }
        }
        else
            for(int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                bullets.Remove(bullets[i]);

                if (b != null)
                    GameObject.Destroy(b.gameObject);
            }
    }
}
