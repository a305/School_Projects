using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAsteroid : MonoBehaviour
{
    public List<Asteroid> parts;
    public TheWorld world;
    private float numberOfSteps = 8f;
	// Use this for initialization
	void Start ()
    {
        foreach (Asteroid ast in parts)
        {
            ast.timeToLive = 10000f;
            ast.Initialize(world);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(world.TheRoot.transform);
        transform.position += (world.TheRoot.transform.position - transform.position) * Time.deltaTime / numberOfSteps; 
        for(int i = 0; i < parts.Count; i++)
        {
            Asteroid ast = parts[i];
            bool isCollided = world.ProcessCollision(ast.transform, world.TheRoot.transform);
            if (isCollided)
                Debug.Log("You are DEAD M9");
            else
                Debug.Log("Coming after you");
        }
	}
}
