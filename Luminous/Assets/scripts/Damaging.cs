using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour {

    [SerializeField]
    float damageAmount;

    [SerializeField]
    bool damageByForce;

    Rigidbody2D rb;

    private PlayerController playerController;
    void Start()
    {
        if (damageByForce) rb = gameObject.GetComponent<Rigidbody2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerDamage")
        {
            print(collision.gameObject.name);

            playerController.setDamage(getDamage());
        }
    }

    private float getDamage()
    {
        if (!damageByForce) return damageAmount;
        float velocity = rb.velocity.magnitude;
        return velocity;
    }

}
