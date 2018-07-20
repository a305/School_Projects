using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armFollow : MonoBehaviour {

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject armPivot;

    [SerializeField]
    GameObject arm;

    [SerializeField] Transform armPositionLeft, armPositionRight, 
        armPositionLeftCrouch, armPositionRightCrouch;
    // Use this for initialization
    void Start () {

    
    }
	
	// Update is called once per frame
	void Update () {


        if (player.GetComponent<SpriteRenderer>().flipX)
        {
            if (!player.GetComponent<PlayerController>().isCrouching())
            {
                armPivot.transform.localPosition = armPositionRight.localPosition;
            }
            else
            {
                armPivot.transform.localPosition = armPositionRightCrouch.localPosition;
            }

                arm.GetComponent<SpriteRenderer>().flipX = true;
            if (armPivot.transform.eulerAngles.z > 0 && armPivot.transform.eulerAngles.z <= 180)
            {
                arm.GetComponent<SpriteRenderer>().flipX = false;

            }
        }
        else
        {
            if (!player.GetComponent<PlayerController>().isCrouching())
            {
                armPivot.transform.localPosition = armPositionLeft.localPosition;
            }
            else
            {
                armPivot.transform.localPosition = armPositionLeftCrouch.localPosition;
            }
            arm.GetComponent<SpriteRenderer>().flipX = false;
            if (armPivot.transform.eulerAngles.z > 0 && armPivot.transform.eulerAngles.z <= 180)
            {
                arm.GetComponent<SpriteRenderer>().flipX = true;

            }

        }
    }
}
