// Nathan Pham
// CS451 Autumn 2017
// 12/9/2017
// This script models the behavior of the cannon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform barrel;
    public bool active = false;
    public TheWorld world;
    float bulletGen = 0.25f;
    float currentGen = 0f;
	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            currentGen += Time.deltaTime;
            if (Input.GetKey(KeyCode.Z) && currentGen >= bulletGen)
            {
                GameObject bullet = Instantiate(Resources.Load("Prefabs\\PlayerBullet")) as GameObject;
                bullet.transform.localPosition = transform.position + 2 * barrel.up;
                bullet.GetComponent<PlayerBullet>().SetSpeed(10f);
                bullet.GetComponent<PlayerBullet>().SetTravellingDirection(barrel.up);
                bullet.GetComponent<PlayerBullet>().SetWorld(ref world);
                currentGen = 0f;
            }
        }
	}
}
