using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentLife += Time.deltaTime;
        transform.localPosition += travellingNormal * speed * Time.deltaTime; // travel

        if (world.existingSentries.Count > 0) // test collision with every existing sentry
        {
            for (int i = 0; i < world.existingSentries.Count; i++)
            {
                StationarySentry sentry = world.existingSentries[i];
                bool isCollide = world.ProcessCollision(transform, sentry.gameObject.transform);
                if (isCollide)
                {
                    //Debug.Log("Player Bullet Hit");
                    sentry.enabled = false;
                    //world.existingSentries.Remove(world.existingSentries[i]);
                    //GameObject.Destroy(sentry.gameObject);
                }
                //else
                //    Debug.Log("Player Bullet not hit");
            }
        }

        if (world.asteroids.Count > 0)
        {
            for (int j = 0; j < world.asteroids.Count; j++)
            {
                Asteroid ast = world.asteroids[j];
                bool isCollide = world.ProcessCollision(transform, world.asteroids[j].gameObject.transform);
                if (isCollide)
                {
                    world.asteroids.Remove(world.asteroids[j]);
                    //Debug.Log("Asteroid destroyed");
                    GameObject.Destroy(ast.gameObject);
                }
            }
        }

        if (world.giantAsteroids.Count > 0)
        {
            for (int i = 0; i < world.giantAsteroids.Count; i++)
            {
                GiantAsteroid ga = world.giantAsteroids[i];
                for (int j = 0; j < ga.parts.Count; j++)
                {
                    Asteroid ast = ga.parts[j];
                    bool isCollide = world.ProcessCollision(transform, ga.parts[j].gameObject.transform);
                    if (isCollide)
                    {
                        ga.parts.Remove(ga.parts[j]);
                        //Debug.Log("Giant Asteroid part destroyed");
                        GameObject.Destroy(ast.gameObject);
                    }
                }

                if (ga.parts.Count <= 0)
                {
                    world.giantAsteroids.Remove(world.giantAsteroids[i]);
                    GameObject.Destroy(ga.gameObject);
                }

            }
        }

        if (currentLife >= timeToLive)
            Destroy(gameObject);
    }
}

