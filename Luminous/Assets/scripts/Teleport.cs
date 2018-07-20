using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour {

    [SerializeField]
    Camera mainCamera;
    public GameObject teleportUI;
    public float coolDownStart = 3;
    public float range = 20;

    float coolDown;
    private bool canTeleport;
    private Vector3 lastCameraPos;
    private FlashLightControl flashLight;
    private float teleportDistance;

    // Use this for initialization
    void Start ()
    {
        coolDown = 0;
        flashLight = transform.Find("armPivot").Find("arm").GetComponent<FlashLightControl>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If coolDown is greater than 0, decrement time
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            teleportUI.GetComponent<Text>().text = string.Format("Can teleport in: {0:0.00}", Mathf.Clamp(coolDown, 0, Mathf.Infinity));
        }
        else if (teleportUI.activeSelf)
            teleportUI.SetActive(false);

        // Triggered while left mouse button is held down, performs are necessary checks for teleport
        if (Input.GetMouseButton(0) && coolDown <= 0)
        {
            // Activate teleport UI if currently deactivated
            if (teleportUI.activeInHierarchy == false)
                teleportUI.SetActive(true);

            // Get new camera view size
            float newCameraSize = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - lastCameraPos).magnitude * .02f;

            // Update camera view if view is within range
            if (mainCamera.orthographicSize < range)
            {
                mainCamera.orthographicSize += newCameraSize;
                lastCameraPos = mainCamera.transform.position;
            }

            // If teleportUI is currently inactive, activate it
            if (teleportUI.activeSelf == false)
                teleportUI.SetActive(true);

            // Raycast between player position and mouse position
            Vector2 difference = (mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            Vector2 dir = difference.normalized; // Normalize difference to get direction
            teleportDistance = difference.magnitude; // Use magnitude of difference to get required length of raycast
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, teleportDistance);

            // Run through all collided objects and see if any are tagged to "BlockTeleport", if true don't teleport player
            bool blocked = false;
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.tag == "BlockTeleport")
                    blocked = true;
            }

            // If player doesn't have enough battery life to execute teleport, block teleport
            if (flashLight.GetBattery() - teleportDistance < 0)
                blocked = true;

            // Set canTeleport to true if there are no obstructions
            if (blocked == false)
            {
                teleportUI.GetComponent<Text>().text = "Can teleport";
                canTeleport = true;
            }
            // Update teleport indicator UI to display not enough battery message if such is the case
            else if (flashLight.GetBattery() - teleportDistance < 0)
            {
                teleportUI.GetComponent<Text>().text = "Unable to teleport - not enough battery";
                canTeleport = false;
            }
            // Update teleport indicator UI to display obstruction message if one exists
            else
            {
                teleportUI.GetComponent<Text>().text = "Unable to teleport - obstruction";
                canTeleport = false;
            }
        }
        // Move camera back towards player
        else if (mainCamera.orthographicSize > 5)
            mainCamera.orthographicSize -= .5f;

        // Triggered on left mouse button up, teleports player if possible and resets values used for teleport
        if (Input.GetMouseButtonUp(0))
        {
            // If canTeleport is set to true, teleport
            if (canTeleport)
            {
                teleportPlayer();
                coolDown = coolDownStart;
                teleportUI.SetActive(true);
            }

            // Reset canTeleport status
            canTeleport = false;
        }
    }

    public void teleportPlayer()
    {
        flashLight.SetBattery(-teleportDistance);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}

