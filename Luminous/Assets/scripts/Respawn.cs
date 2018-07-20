using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform respawnpt;

    void OnTriggerEnter2D(Collider2D other)
    {
		if(other.tag == "Player")
        player.transform.position = respawnpt.transform.position;
    }
}
