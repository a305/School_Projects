// Nathan Pham
// CS451 Autumn 2017
// November 23rd, 2017
// This Script is used to control the manipulations done for a specific camera
//#define DEBUG_ON
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraManipulation camManip;
    private const float kPixelToDegree = 0.1f;
    private const float kPixelToDistant = 0.05f;
	
	// Update is called once per frame
	void Update ()
    {
#if DEBUG_ON
        Debug.Log("zoomDir " + zoomDir);
#endif
        // this will change the rotation
        camManip.cam.transform.LookAt(camManip.lookAt.transform);

        // When left alt is pressed and either mouse buttons are pressed
        if (Input.GetKey(KeyCode.LeftAlt) &&
( Input.GetMouseButtonDown(0) || (Input.GetMouseButtonDown(1)) ))
        {
            // Initializes the mouse positions for the camera manipulation
            camManip.SetMouseX(Input.mousePosition.x);
            camManip.SetMouseY(Input.mousePosition.y);

            // Debug.Log("MouseButtonDown 0: (" + mMouseX + " " + mMouseY);
        }


        else if (Input.GetKey(KeyCode.LeftAlt) &&
                ( Input.GetMouseButton(0) || (Input.GetMouseButton(1)) ) )
        {
            // Get the change in the old mouse positition and the new mouse position
            float dx = camManip.GetMouseX() - Input.mousePosition.x;
            float dy = camManip.GetMouseY() - Input.mousePosition.y;

            // Set the camera's mouse positions for calculation to the current mouse position
            camManip.SetMouseX(Input.mousePosition.x);
            camManip.SetMouseY(Input.mousePosition.y);

            // left click
            if (Input.GetMouseButton(0)) // Camera orbiting (tumbling)
            {
                camManip.RotateCameraAboutUp(-dx * kPixelToDegree);
                camManip.RotateCameraAboutSide(dy * kPixelToDegree);
            }

            // right click
            else if (Input.GetMouseButton(1)) // Camera tracking
                camManip.PerformCameraTracking(dx, dy,kPixelToDistant);
        }

        if (Input.GetKey(KeyCode.LeftAlt))  // Camera dolly (zooming)
        {
            float zoomDir = Input.GetAxis("Mouse ScrollWheel");
            if (zoomDir > 0)
                camManip.PerformDolly(1);
            else if (zoomDir < 0)
                camManip.PerformDolly(-1);
        }
    }
}
